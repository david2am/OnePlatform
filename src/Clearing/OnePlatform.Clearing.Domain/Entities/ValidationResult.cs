using OnePlatform.Clearing.Domain.Enums;

namespace OnePlatform.Clearing.Domain.Entities;

public sealed class ValidationResult : Entity
{
  private ValidationResult() : base(Guid.Empty) {}

  private ValidationResult(
    Guid id,
    Guid paymentId,
    ValidationCheckType checkType,
    ValidationOutcome outcome,
    DateTimeOffset checkedAt
  ) : base(id)
  {
    
  }

  public Guid PaymentId { get; private set; }
  public ValidationCheckType CheckType { get; private set; }
  public ValidationOutcome Outcome { get; private set; }
  public DateTimeOffset CheckedAt { get; private set; }

  public bool Passed => Outcome == ValidationOutcome.Pass;

  internal static ValidationResult Create(
    Guid paymentId,
    ValidationCheckType checkType,
    ValidationOutcome outcome,
    DateTimeOffset checkedAt)
  {
    if (paymentId == Guid.Empty)
      throw new ArgumentException("Payment Id is required.", nameof(paymentId));

    return new(Guid.NewGuid(), paymentId, checkType, outcome, checkedAt);
  }
}