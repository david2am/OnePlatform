using OnePlatform.Clearing.Domain.Enums;
using OnePlatform.Clearing.Domain.Shared;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Domain.Entities;

public sealed class Participant : Entity<ParticipantId>
{
  public PaymentId PaymentId { get; private set; }
  public InstitutionId InstitutionId { get; private set; }
  public AccountId AccountId { get; private set; }
  public ParticipantRole Role { get; private set; }
  public ParticipantStatus Status { get; private set; }

  private Participant() : base (null!)
  {
    PaymentId = null!;
    InstitutionId = null!;
    AccountId = null!;
  }
  private Participant(
    ParticipantId id,
    PaymentId paymentId,
    InstitutionId institutionId,
    AccountId accountId,
    ParticipantRole role,
    ParticipantStatus status
  )
    : base (id)
  {
    PaymentId = paymentId;
    InstitutionId = institutionId;
    AccountId = accountId;
    Role = role;
    Status = status;
  }

  public static Result<Participant> Add(
    PaymentId paymentId,
    InstitutionId institutionId,
    AccountId accountId,
    ParticipantRole role
  )
  {
    if (paymentId.Value == Guid.Empty)
      return Result.Failure<Participant>(
          ParticipantErrors.InvalidPaymentId
      );

    if (institutionId.Value == Guid.Empty)
      return Result.Failure<Participant>(
          ParticipantErrors.InvalidInstitutionId
      );

    if (accountId.Value == Guid.Empty)
      return Result.Failure<Participant>(
          ParticipantErrors.InvalidAccountId
      );

    if (!Enum.IsDefined(role))
      return Result.Failure<Participant>(
          ParticipantErrors.InvalidRole
      );

    var participant = new Participant(
      ParticipantId.New(),
      paymentId,
      institutionId,
      accountId,
      role,
      ParticipantStatus.Processing
    );

    return Result.Success(participant);
  }
}

public static class ParticipantErrors
{
  public static readonly Error InvalidPaymentId = new(
    "Participants.InvalidPaymentId", "Payment id is required.");

  public static readonly Error InvalidInstitutionId = new(
    "Participants.InvalidInstitutionId", "Institution id is required.");

  public static readonly Error InvalidAccountId = new(
    "Participants.InvalidAccountId", "Account id is required.");

  public static readonly Error InvalidRole = new(
    "Participants.InvalidRole", "Role is required.");
}