using OnePlatform.Clearing.Domain.DomainEvents;
using OnePlatform.Clearing.Domain.Enums;
using OnePlatform.Clearing.Domain.Shared;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Domain.Entities;

public sealed class Payment : AggregateRoot<PaymentId>
{

  // ISO 20022 message identifiers.
  // MssgId: like an envelope with several letters.
  //  Identifies a msg that could carry several payments,
  //  it's assigned by the sender of the message (the institution)
  public MessageId MessageId { get; private set; }
  // E2EId: it's the payment's tracking number.
  //  identifies a payment, it's created by the debtor
  //  from it's Institution system
  //  unchanged accross the payment chain
  public EndToEndId EndToEndId { get; private set; }
  // returf_of another PaymentId
  public PaymentId? ReturnOf { get; private set; }

  public Money Amount { get; private set; }

  // not always the debtor InstitutionId is the SenderInstitutionId,
  // for example when a return happen from creditor's side.
  // A MessageId is only unique per sender, and this is how we verify it
  public InstitutionId SenderInstitutionId { get; private set; }
  public PaymentParty Debtor { get; private set; }
  public PaymentParty Creditor { get; private set; }

  public PaymentType PaymentType { get; private set; }
  public PaymentStatus Status { get; private set; }
  public DateTimeOffset InitiatedAt { get; private set; }

  private readonly List<Participant> _participants = [];
  public IReadOnlyCollection<Participant> Participants => _participants.AsReadOnly();

  private readonly List<ValidationResult> _validationResults = [];
  public IReadOnlyCollection<ValidationResult> ValidationResults => _validationResults.AsReadOnly();

  private readonly List<StatusEvent> _statusEvents = [];
  public IReadOnlyCollection<StatusEvent> StatusEvents => _statusEvents.AsReadOnly();

  // protects against two processes changing the same payment at once
  public byte[] RowVersion { get; private set; } = [];

  private Payment() : base(null!) {
    MessageId = null!;
    EndToEndId = null!;
    Amount = null!;
    Debtor = null!;
    Creditor = null!;
    Status = PaymentStatus.Pending;
  }

  private Payment(
    PaymentId id,
    MessageId messageId,
    EndToEndId endToEndId,
    Money amount,
    PaymentParty debtor,
    PaymentParty creditor,
    IReadOnlyCollection<Participant> participants,
    DateTimeOffset initiatedAt
  ) : base(id)
  {
    MessageId = messageId;
    EndToEndId = endToEndId;
    Amount = amount;
    Debtor = debtor;
    Creditor = creditor;

    _participants.AddRange(participants);

    InitiatedAt = initiatedAt;
    PaymentType = PaymentType.CreditTransfer;
    Status = PaymentStatus.Pending;
  }

  public static Result<Payment> Receive(
    MessageId messageId,
    EndToEndId endToEndId,
    Money amount,
    PaymentParty debtor,
    PaymentParty creditor,
    IReadOnlyCollection<ParticipantInput> participants,
    DateTimeOffset initiatedAt,
    DateTimeOffset receivedAt
  )
  {
    if (messageId is null)
      return Result.Failure<Payment>(
        PaymentErrors.MessageIdRequired);

    if (endToEndId is null)
      return Result.Failure<Payment>(
        PaymentErrors.IdRequired);

    if (amount is null)
      return Result.Failure<Payment>(PaymentErrors.AmountRequired);

    if (debtor is null || creditor is null)
      return Result.Failure<Payment>(PaymentErrors.PartyRequired);

    if (debtor.InstitutionId == creditor.InstitutionId && debtor.AccountNumber == creditor.AccountNumber)
      return Result.Failure<Payment>(PaymentErrors.SameAccount);

    var participantsValidation = ValidateParticipants(participants);

    if (participantsValidation.IsFailure)
        return Result.Failure<Payment>(participantsValidation.Error);
    
    var paymentId = PaymentId.New();

    var paymentParticipants = new List<Participant>();

    foreach (var participantInput in participants)
    {
        var participantResult = Participant.Add(
            paymentId,
            participantInput.InstitutionId,
            participantInput.AccountId,
            participantInput.Role
        );

        if (participantResult.IsFailure)
            return Result.Failure<Payment>(participantResult.Error);

        paymentParticipants.Add(participantResult.Value);
    }

    var payment = new Payment(
      paymentId,
      messageId,
      endToEndId,
      amount,
      debtor,
      creditor,
      paymentParticipants,
      initiatedAt
    );

    payment._statusEvents.Add(
      StatusEvent.Create(
        paymentId,
        PaymentEventType.Received,
        receivedAt,
        null)
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

  private static Result<IReadOnlyCollection<ParticipantInput>>
  ValidateParticipants(
    IReadOnlyCollection<ParticipantInput> participants
  )
  {
    if (participants is null || participants.Count == 0)
        return Result.Failure<IReadOnlyCollection<ParticipantInput>>(
          PaymentErrors.ParticipantsRequired);

    var debtorAgents = participants
        .Where(p => p.Role == ParticipantRole.DebtorAgent)
        .ToList();

    if (debtorAgents.Count != 1)
        return Result.Failure<IReadOnlyCollection<ParticipantInput>>(
          PaymentErrors.InvalidDebtorAgentCount);

    var creditorAgents = participants
        .Where(p => p.Role == ParticipantRole.CreditorAgent)
        .ToList();

    if (creditorAgents.Count != 1)
        return Result.Failure<IReadOnlyCollection<ParticipantInput>>(
          PaymentErrors.InvalidCreditorAgentCount);

    var duplicates = participants
        .GroupBy(p => new { p.InstitutionId, p.Role })
        .Any(g => g.Count() > 1);

    if (duplicates)
        return Result.Failure<IReadOnlyCollection<ParticipantInput>>(
          PaymentErrors.DuplicateParticipants);

    return Result.Success(participants);
  }

  public Result RecordValidationResult(
    ValidationCheckType checkType,
    ValidationOutcome outcome,
    DateTimeOffset checkedAt)
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

    RaiseDomainEvent( // TODO: validate later
      new PaymentRecordedValidationDomainEvent(
        checkType,
        outcome,
        checkedAt
      )
    );

    return Result.Success();
  }

private static readonly ValidationCheckType[] _requiredChecks =
[
    ValidationCheckType.Schema,
    ValidationCheckType.Sanctions,
    ValidationCheckType.Duplicate,
    ValidationCheckType.Membership
];

  public Result CompleteValidation(DateTimeOffset at)
  {
    if (Status != PaymentStatus.Pending)
      return Result.Failure(PaymentErrors.NotPending);

    var allRecorded = _requiredChecks.All(
        required => _validationResults.Any(
            result => result.CheckType == required));

    if (!allRecorded)
        return Result.Failure(PaymentErrors.ValidationIncomplete);

    var failedChecks = _validationResults
        .Where(result =>
            _requiredChecks.Contains(result.CheckType) &&
            !result.Passed
        )
        .Select(result => result.CheckType)
        .ToArray();

    if (failedChecks.Length == 0)
    {
      var result = ChangeStatusTo(
          PaymentStatus.Validated,
          PaymentEventType.Validated,
          at
      );

      if (result.IsFailure)
          return result;

      RaiseDomainEvent( // TODO: apply unit of work here
          new PaymentValidationCompletedDomainEvent(Id, at)
      );

      return Result.Success();
    }

    var details = "Failed checks: " + string.Join(", ", failedChecks);

    RaiseDomainEvent( // TODO: validate later
      new PaymentValidationFailedDomainEvent(Id, at)
    );

    return ChangeStatusTo(
      PaymentStatus.Rejected,
      PaymentEventType.Rejected,
      at,
      details
    );
  }

    private static bool CanTransition(
      PaymentStatus current,
      PaymentStatus next) =>
    (current, next) switch
    {
        (PaymentStatus.Pending, PaymentStatus.Validated) => true,
        (PaymentStatus.Pending, PaymentStatus.Rejected) => true,
        (PaymentStatus.Validated, PaymentStatus.Settled) => true,
        (PaymentStatus.Settled, PaymentStatus.Final) => true,
        (PaymentStatus.Settled, PaymentStatus.Returned) => true,
        (PaymentStatus.Final, PaymentStatus.Returned) => true,
        _ => false
    };

  private Result ChangeStatusTo(
    PaymentStatus next,
    PaymentEventType eventType,
    DateTimeOffset at,
    string? details = null)
  {
    if (!CanTransition(Status, next))
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
    "Message Id is required."
  );
  public static readonly Error IdRequired = new(
    "Payment.IdRequired",
    "Id is required."
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
    "The payment is not pending."
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

  public static readonly Error SameAccount = new(
    "Payment.SameAccount",
    "Debtor and creditor cannot be the same account."
  );

  public static readonly Error ParticipantsRequired = new(
    "Payment.ParticipantsRequired",
    "Participants are required."
  );

  public static readonly Error InvalidDebtorAgentCount = new(
      "Payment.InvalidDebtorAgentCount",
      "A payment must have exactly one debtor agent."
  );

  public static readonly Error InvalidCreditorAgentCount = new(
      "Payment.InvalidCreditorAgentCount",
      "A payment must have exactly one creditor agent."
  );

  public static readonly Error DuplicateParticipants = new(
      "Payment.DuplicateParticipants",
      "An institution cannot participate more than once with the same role."
  );

  public static readonly Error ConcurrencyConflict = new(
    "Payment.ConcurrencyConflict",
    "The payment was modified by another process. Reload it and try again.");
}