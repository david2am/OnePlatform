using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Clearing.Domain.ValueObjects;

public sealed record Money
{
  public decimal Amount { get; }
  public string Currency { get; } = string.Empty;

  private Money(decimal amount, string currency)
  {
    Amount = amount;
    Currency = currency;
  }

  public static Result<Money> Create(decimal amount, string currency)
  {
    if (amount <= 0)
      return Result.Failure<Money>(MoneyErrors.NotPositive);

    var code = currency?.Trim().ToUpperInvariant();
    if (string.IsNullOrWhiteSpace(currency) || code is not { Length: 3 } || !code.All(c => c is >= 'A' and <= 'Z'))
      return Result.Failure<Money>(MoneyErrors.InvalidCurrency);

    return Result.Success<Money>(new (amount, code));
  }
}

public static class MoneyErrors
{
  public static readonly Error NotPositive = new(
    "MoneyErrors.NotPositive",
    "Amount must be greater than zero."
  );

  public static readonly Error InvalidCurrency = new(
    "MoneyErrors.InvalidCurrency",
    "Currency must be a 3-letter ISO code."
  );
}