using OnePlatform.Clearing.Shared.Domain.Exceptions;

namespace OnePlatform.Clearing.Shared.Domain.ValueObjects;

public sealed record Currency
{
  public string Code { get; } = string.Empty;
  private Currency(){}
  private Currency(string code)
  {
    Code = code;
  }

  public static Result<Currency> Create(string code)
  {
    var normalized = code?.Trim().ToUpperInvariant();

    if (string.IsNullOrWhiteSpace(normalized)
      || normalized.Length != 3
      || !normalized.All(c => c is >= 'A' and <= 'Z')
    )
      return Result.Failure<Currency>(
          CurrencyErrors.InvalidCode
      );

    return Result.Success(new Currency(normalized));
  }
}

public static class CurrencyErrors
{
  public static readonly Error InvalidCode = new(
    "CurrencyErrors.InvalidCode",
    "Currency must be a 3-letter ISO code, for example USD."
  );
}