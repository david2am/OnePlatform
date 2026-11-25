using OnePlatform.Clearing.Domain.Enums;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Domain.Entities;

public sealed class ValidationResult : Entity<Guid>
{
  private ValidationResult() : base(Guid.Empty)
  {
    PaymentId = null!;
  }

  private ValidationResult(
    Guid id,
    PaymentId paymentId,
    ValidationCheckType checkType,
    ValidationOutcome outcome,
    DateTimeOffset checkedAt
  ) : base(id)
  {
    PaymentId = paymentId;
    CheckType = checkType;
    Outcome = outcome;
    CheckedAt = checkedAt;
  }

  public PaymentId PaymentId { get; private set; }
  public ValidationCheckType CheckType { get; private set; }
  public ValidationOutcome Outcome { get; private set; }
  public DateTimeOffset CheckedAt { get; private set; }

  public bool Passed => Outcome == ValidationOutcome.Pass;

  internal static ValidationResult Create(
    PaymentId paymentId,
    ValidationCheckType checkType,
    ValidationOutcome outcome,
    DateTimeOffset checkedAt)
  {
    if (paymentId is null)
      throw new ArgumentException("Payment Id is required.", nameof(paymentId));

    return new(Guid.CreateVersion7(), paymentId, checkType, outcome, checkedAt);
  }
}