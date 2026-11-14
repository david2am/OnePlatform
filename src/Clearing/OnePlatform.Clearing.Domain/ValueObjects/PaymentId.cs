using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Clearing.Domain.ValueObjects;

public sealed record PaymentId : IGuidId<PaymentId>
{
  public Guid Value { get; }

  private PaymentId(Guid value) => Value = value;

  public static PaymentId FromGuid(Guid value) => new(value);

  public static Result<PaymentId> Create(Guid value) =>
    value == Guid.Empty
      ? Result.Failure<PaymentId>(PaymentIdErrors.Required)
      : Result.Success(new PaymentId(value));

  public static PaymentId New() => new(Guid.CreateVersion7());
}

public static class PaymentIdErrors
{
    public static readonly Error Required = new(
        "PaymentId.Required",
        "PaymentId is required.");
}