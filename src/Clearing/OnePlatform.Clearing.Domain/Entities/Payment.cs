using OnePlatform.Clearing.Domain.DomainEvents;
using OnePlatform.Clearing.Domain.Enums;
using OnePlatform.Clearing.Domain.Shared;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Domain.Entities;

public sealed class Payment : AggregateRoot
{
  // ISO 20022 Max35Text
  private const int MaxIdLength = 35;

  private Payment() : base(Guid.Empty) {
    MessageId = string.Empty;
    EndToEndId = string.Empty;
    Amount = null!;
    Debtor = null!;
    Creditor = null!;
  }

  private Payment(
    Guid id,
    string messageId,
    string endToEndId,
    Money amount,
    PartyDetails debtor,
    PartyDetails creditor,
    DateTime initiatedAt
  ) : base(id)
  {
    MessageId = messageId;
    EndToEndId = endToEndId;
    Amount = amount;
    Debtor = debtor;
    Creditor = creditor;
    InitiatedAt = initiatedAt;
    PaymentType = PaymentType.CreditTransfer;
    Status = PaymentStatus.Pending;
  }

  // ISO 20022 MsgId from the pacs.008 message (unique).
  public string MessageId { get; private set; }
  public string EndToEndId { get; private set; }

  public Money Amount { get; private set; }
  public PartyDetails Debtor { get; private set; }
  public PartyDetails Creditor { get; private set; }

  public PaymentType PaymentType { get; private set; }
  public PaymentStatus Status { get; private set; }
  public DateTime InitiatedAt { get; private set; }

  private readonly List<ValidationResult> _validationResults = [];
  private readonly List<StatusEvent> _statusEvents = [];

  public IReadOnlyCollection<ValidationResult> ValidationResults => _validationResults;
  public IReadOnlyCollection<StatusEvent> StatusEvents => _statusEvents;

  public static Result<Payment> Receive(
    string messageId,
    string endToEndId,
    Money amount,
    PartyDetails debtor,
    PartyDetails creditor,
    DateTime initiatedAt,
    DateTime receivedAt
  )
  {
    var msgId = messageId?.Trim();
    var e2eId = endToEndId?.Trim();

    if (string.IsNullOrEmpty(msgId))
      return Result.Failure<Payment>(PaymentErrors.MessageIdRequired);

    if (msgId.Length > MaxIdLength)
      return Result.Failure<Payment>(PaymentErrors.MessageIdTooLong);

    if (string.IsNullOrEmpty(e2eId))
      return Result.Failure<Payment>(PaymentErrors.EndToEndIdRequired);

    if (e2eId.Length > MaxIdLength)
      return Result.Failure<Payment>(PaymentErrors.EndToEndIdTooLong);

    if (amount is null)
      return Result.Failure<Payment>(PaymentErrors.AmountRequired);

    if (debtor is null || creditor is null)
      return Result.Failure<Payment>(PaymentErrors.PartyRequired);
    
    var payment = new Payment(
      Guid.NewGuid(),
      msgId,
      e2eId,
      amount,
      debtor,
      creditor,
      initiatedAt
    );

    payment._statusEvents.Add(
      StatusEvent.Create(payment.Id, PaymentEventType.Received, receivedAt, "")
    );

    payment.RaiseDomainEvent(
      new PaymentReceivedDomainEvent(
        payment.Id,
        PaymentEventType.Received,
        receivedAt,
        null)
    );

    return Result.Success(payment);
  }

  public Result RecordValidationResult(
    ValidationCheckType checkType,
    ValidationOutcome outcome,
    DateTime checkedAt)
  {
    if (!Enum.IsDefined(checkType))
      return Result.Failure(PaymentErrors.InvalidCheckType);

    if (!Enum.IsDefined(outcome))
      return Result.Failure(PaymentErrors.InvalidOutcome);

    if (Status != PaymentStatus.Pending)
      return Result.Failure(PaymentErrors.NotPending);

    if (_validationResults.Any(v => v.CheckType == checkType))
      return Result.Failure(PaymentErrors.CheckAlreadyRecorded);

    _validationResults.Add(ValidationResult.Create(Id, checkType, outcome, checkedAt));

    return Result.Success();
  }

  public Result CompleteValidation(DateTime at)
  {
    if (Status != PaymentStatus.Pending)
      return Result.Failure(PaymentErrors.NotPending);

    var allRecorded = Enum.GetValues<ValidationCheckType>()
      .All(type => _validationResults.Any(v => v.CheckType == type));

    if (!allRecorded)
      return Result.Failure(PaymentErrors.ValidationIncomplete);

    var failed = _validationResults.Where(v => !v.Passed).ToList();

    if (failed.Count == 0)
      return ChangeStatusTo(PaymentStatus.Validated, PaymentEventType.Validated, at);

    var details = "Failed checks: " + string.Join(", ", failed.Select(v => v.CheckType));
    return ChangeStatusTo(PaymentStatus.Rejected, PaymentEventType.Rejected, at, details);
  }

  private static readonly IReadOnlyDictionary<PaymentStatus, PaymentStatus[]> AllowedTransitions
    = new Dictionary<PaymentStatus, PaymentStatus[]>
    {
      [PaymentStatus.Pending]   = [PaymentStatus.Validated, PaymentStatus.Rejected],
      [PaymentStatus.Validated] = [PaymentStatus.Settled],
      [PaymentStatus.Settled]   = [PaymentStatus.Final, PaymentStatus.Returned],
      [PaymentStatus.Final]     = [PaymentStatus.Returned],
      [PaymentStatus.Rejected]  = [],
      [PaymentStatus.Returned]  = []
    };

  private Result ChangeStatusTo(
    PaymentStatus next,
    PaymentEventType eventType,
    DateTime at,
    string? details = null)
  {
    if (!AllowedTransitions[Status].Contains(next))
      return Result.Failure(PaymentErrors.InvalidTransition);

    Status = next;
    _statusEvents.Add(StatusEvent.Create(Id, eventType, at, details));

    return Result.Success();
  }
}

public static class PaymentErrors
{
  public static readonly Error InvalidCheckType = new(
    "Payment.InvalidCheckType",
    "Validation check type is not valid."
  );

  public static readonly Error InvalidOutcome = new(
    "Payment.InvalidOutcome",
    "Validation outcome is not valid."
  );

  public static readonly Error MessageIdRequired = new(
    "Payment.MessageIdRequired",
    "Message id is required."
  );
  
  public static readonly Error MessageIdTooLong = new(
    "Payment.MessageIdTooLong",
    "Message id is too long."
  );

  public static readonly Error EndToEndIdRequired = new(
    "Payment.EndToEndIdRequired",
    "End-to-end id is required."
  );
  
  public static readonly Error EndToEndIdTooLong = new(
    "Payment.EndToEndIdTooLong",
    "End-to-end id is too long."
  );
  
  public static readonly Error AmountRequired = new(
    "Payment.AmountRequired",
    "Amount is required."
  );
  
  public static readonly Error PartyRequired = new(
    "Payment.PartyRequired",
    "Party is required."
  );

  public static readonly Error NotPending = new(
    "Payment.NotPending",
    "Only a pending payment can be validated."
  );

  public static readonly Error CheckAlreadyRecorded = new(
    "Payment.CheckAlreadyRecorded",
    "This validation check was already recorded."
  );

  public static readonly Error ValidationIncomplete = new(
    "Payment.ValidationIncomplete",
    "All validation checks must be recorded first."
  );

  public static readonly Error InvalidTransition = new(
    "Payment.InvalidTransition",
    "This status change is not allowed."
  );
}