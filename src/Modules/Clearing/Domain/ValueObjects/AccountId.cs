using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Clearing.Domain.ValueObjects;

public sealed record AccountId : IGuidId<AccountId>
{
  public Guid Value { get; }

  private AccountId(Guid value) => Value = value;

  public static AccountId FromGuid(Guid value) => new(value);

  public static Result<AccountId> Create(Guid value) =>
    value == Guid.Empty
      ? Result.Failure<AccountId>(AccountIdErrors.Required)
      : Result.Success(new AccountId(value));

  public static AccountId New() => new(Guid.CreateVersion7());
}

public static class AccountIdErrors
{
    public static readonly Error Required = new(
        "AccountId.Required",
        "AccountId is required.");
}