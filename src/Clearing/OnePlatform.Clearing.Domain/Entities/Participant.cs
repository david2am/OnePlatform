using OnePlatform.Clearing.Domain.Shared;
using OnePlatform.Clearing.Domain.Enums;

namespace OnePlatform.Clearing.Domain.Entities;

public sealed class Participant : Entity
{
  // Only for EF Core.
  private Participant() : base(Guid.Empty)
  {
  }

  private Participant(
    Guid id,
    Guid paymentId,
    Guid institutionId,
    ParticipantRole role) : base(id)
  {
    PaymentId = paymentId;
    InstitutionId = institutionId;
    Role = role;
  }

  // The payment this participant belongs to.
  public Guid PaymentId { get; private set; }

  // The institution (credit union) playing the role. Never a retail member.
  public Guid InstitutionId { get; private set; }

  public ParticipantRole Role { get; private set; }

  public bool IsDebit => Role == ParticipantRole.Debit;
  public bool IsCredit => Role == ParticipantRole.Credit;

  public static Result<Participant> Create(
    Guid paymentId,
    Guid institutionId,
    ParticipantRole role)
  {
    if (paymentId == Guid.Empty)
      return Result.Failure<Participant>(ParticipantErrors.PaymentRequired);

    if (institutionId == Guid.Empty)
      return Result.Failure<Participant>(ParticipantErrors.InstitutionRequired);

    if (!Enum.IsDefined(role))
      return Result.Failure<Participant>(ParticipantErrors.InvalidRole);

    return Result.Success(
      new Participant(Guid.NewGuid(), paymentId, institutionId, role));
  }
}

public static class ParticipantErrors
{
  public static readonly Error PaymentRequired = new(
    "Participant.PaymentRequired", "Payment id is required.");

  public static readonly Error InstitutionRequired = new(
    "Participant.InstitutionRequired", "Institution id is required.");

  public static readonly Error InvalidRole = new(
    "Participant.InvalidRole", "Role must be Debit or Credit.");
}