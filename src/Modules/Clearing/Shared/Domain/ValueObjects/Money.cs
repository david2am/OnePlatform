using OnePlatform.Clearing.Shared.Domain.Exceptions;

namespace OnePlatform.Clearing.Shared.Domain.ValueObjects;

public sealed record Money
{
  // minor units
  public long Amount { get; }
  public Currency Currency { get; }

  private Money()
  {
    Currency = null!;
  }
  private Money(long amount, Currency currency)
  {
    Amount = amount;
    Currency = currency;
  }

  public static Result<Money> Create(long amount, Currency currency)
  {
    if (amount <= 0)
      return Result.Failure<Money>(MoneyErrors.NotPositive);

    if (currency is null)
      return Result.Failure<Money>(MoneyErrors.CurrencyRequired);

    return Result.Success<Money>(new (amount, currency));
  }
}

public static class MoneyErrors
{
  public static readonly Error NotPositive = new(
    "MoneyErrors.NotPositive",
    "Amount must be greater than zero."
  );
  public static readonly Error CurrencyRequired = new(
    "MoneyErrors.CurrencyRequired",
    "Currency type is required."
  );
}