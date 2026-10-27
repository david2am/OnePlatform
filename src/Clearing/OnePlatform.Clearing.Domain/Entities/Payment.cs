using OnePlatform.Clearing.Domain.DomainEvents;
using OnePlatform.Clearing.Domain.Enums;
using OnePlatform.Clearing.Domain.Shared;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Domain.Entities;

public sealed class Payment : AggregateRoot
{
  private Payment() : base(Guid.Empty) {
    MessageId = string.Empty;
    EndToEndId = string.Empty;
    Currency = string.Empty;
    Payer = null!;
    Payee = null!;
  }

  private Payment(
    Guid id,
    string messageId,
    string endToEndId,
    Guid payerInstitutionId,
    Guid payeeInstitutionId,
    PartyDetails payer,
    PartyDetails payee,
    long amountMinor,
    string currency,
    DateTime initiatedAt
  ) : base(id)
  {
    MessageId = messageId;
    EndToEndId = endToEndId;
    PayerInstitutionId = payerInstitutionId;
    PayeeInstitutionId = payeeInstitutionId;
    Payer = payer;
    Payee = payee;
    AmountMinor = amountMinor;
    Currency = currency;
    InitiatedAt = initiatedAt;
    PaymentType = PaymentType.CreditTransfer;
    Status = PaymentStatus.Pending;
  }

  // ISO 20022 MsgId from the pacs.008 message (unique).
  public string MessageId { get; private set; }
  public string EndToEndId { get; private set; }

  public Guid PayerInstitutionId { get; private set; }
  public Guid PayeeInstitutionId { get; private set; }

  public PartyDetails Payer { get; private set; }
  public PartyDetails Payee { get; private set; }

  public long AmountMinor { get; private set; }

  public string Currency { get; private set; }
  public PaymentType PaymentType { get; private set; }
  public PaymentStatus Status { get; private set; }
  public DateTime InitiatedAt { get; private set; }

  private readonly List<Participant> _participants = [];
  private readonly List<ValidationResult> _validationResults = [];
  private readonly List<StatusEvent> _statusEvents = [];

  public IReadOnlyCollection<Participant> Participants => _participants;
  public IReadOnlyCollection<ValidationResult> ValidationResults => _validationResults;
  public IReadOnlyCollection<StatusEvent> StatusEvents => _statusEvents;

  public static Result<Payment> Receive(
    string messageId,
    string endToEndId,
    Guid payerInstitutionId,
    PartyDetails payer,
    Guid payeeInstitutionId,
    PartyDetails payee,
    long amountMinor,
    string currency,
    DateTime initiatedAt,
    DateTime receivedAt
  )
  {
    if (string.IsNullOrWhiteSpace(messageId))
      return Result.Failure<Payment>(PaymentErrors.MessageIdRequired);

    if (string.IsNullOrWhiteSpace(endToEndId))
      return Result.Failure<Payment>(PaymentErrors.EndToEndIdRequired);

    if (payerInstitutionId == Guid.Empty)
      return Result.Failure<Payment>(PaymentErrors.PayerInstitutionRequired);

    if (payeeInstitutionId == Guid.Empty)
      return Result.Failure<Payment>(PaymentErrors.PayeeInstitutionRequired);

    if (payer is null || payee is null)
      return Result.Failure<Payment>(PaymentErrors.PartyRequired);

    if (amountMinor <= 0)
      return Result.Failure<Payment>(PaymentErrors.InvalidAmount);

    var normalizedCurrency = currency?.Trim().ToUpperInvariant() ?? string.Empty;

    if (normalizedCurrency.Length != 3 || !normalizedCurrency.All(char.IsAsciiLetterUpper))
      return Result.Failure<Payment>(PaymentErrors.InvalidCurrency);
    
    var payment = new Payment(
      Guid.NewGuid(),
      messageId.Trim(),
      endToEndId.Trim(),
      payerInstitutionId,
      payeeInstitutionId,
      payer,
      payee,
      amountMinor,
      normalizedCurrency,
      initiatedAt
    );

    var debit = Participant.Create(
      payment.Id,
      payerInstitutionId,
      ParticipantRole.Debit
    );
    if (debit.IsFailure)
      return Result.Failure<Payment>(debit.Error);

    var credit = Participant.Create(
      payment.Id,
      payeeInstitutionId,
      ParticipantRole.Credit
    );
    if (credit.IsFailure)
      return Result.Failure<Payment>(credit.Error);

    payment._participants.Add(debit.Value);
    payment._participants.Add(credit.Value);
    payment._statusEvents.Add(
      StatusEvent.Create(payment.Id, PaymentEventType.Received, receivedAt, "")
    );

    payment.RaiseDomainEvent(new PaymentReceivedDomainEvent());

    return Result.Success(payment);
  }

  public Result RecordValidationResult(
    ValidationCheckType checkType,
    ValidationOutcome outcome,
    DateTime checketAt)
  {
    if (!Enum.IsDefined(checkType))
      return Result.Failure(PaymentErrors.InvalidCheckType);

    if (!Enum.IsDefined(outcome))
      return Result.Failure(PaymentErrors.InvalidOutcome);

    if (Status != PaymentStatus.Pending)
      return Result.Failure(PaymentErrors.NotPending);

    if (_validationResults.Any(v => v.CheckType == checkType))
      return Result.Failure(PaymentErrors.NotPending);

    _validationResults.Add(ValidationResult.Create(Id, checkType, outcome, checketAt));

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

  public static readonly Error EndToEndIdRequired = new(
    "Payment.EndToEndIdRequired",
    "End-to-end id is required."
  );

  public static readonly Error PayerInstitutionRequired = new(
    "Payment.PayerInstitutionRequired",
    "Payer institution is required."
  );

  public static readonly Error PayeeInstitutionRequired = new(
    "Payment.PayeeInstitutionRequired",
    "Payee institution is required."
  );
  
  public static readonly Error PartyRequired = new(
    "Payment.PartyRequired",
    "Party is required."
  );

  public static readonly Error InvalidAmount = new(
    "Payment.InvalidAmount",
    "Amount must be greater than zero."
  );

  public static readonly Error InvalidCurrency = new(
    "Payment.InvalidCurrency",
    "Currency must be a 3-letter ISO code, for example USD."
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