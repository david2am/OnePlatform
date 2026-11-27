using OnePlatform.Clearing.Shared.Domain.ValueObjects;

namespace OnePlatform.Clearing.Shared.Domain.Tests.ValueObjects;

public sealed class MoneyTests
{
  private static Currency CreateUsd() => Currency.Create("USD").Value;

  [Fact]
  public void Create_WithPositiveAmountAndCurrency_ReturnsSuccess()
  {
    var currency = CreateUsd();
    const long minorUnits = 1500;

    var result = Money.Create(minorUnits, currency);

    Assert.True(result.IsSuccess);
    Assert.Equal(minorUnits, result.Value.Amount);
    Assert.Equal(currency, result.Value.Currency);
  }

  [Theory]
  [InlineData(0)]
  [InlineData(-1)]
  [InlineData(-1500)]
  [InlineData(long.MinValue)]
  public void Create_WithZeroOrNegativeAmount_ReturnsNotPositiveFailure(long invalidAmount)
  {
    var currency = CreateUsd();

    var result = Money.Create(invalidAmount, currency);

    Assert.True(result.IsFailure);
    Assert.Equal(MoneyErrors.NotPositive.Code, result.Error.Code);
    Assert.Equal(MoneyErrors.NotPositive.Message, result.Error.Message);
  }

  [Fact]
  public void Create_WithNullCurrency_ReturnsCurrencyRequiredFailure()
  {
    var result = Money.Create(100, null!);

    Assert.True(result.IsFailure);
    Assert.Equal(MoneyErrors.CurrencyRequired.Code, result.Error.Code);
    Assert.Equal(MoneyErrors.CurrencyRequired.Message, result.Error.Message);
  }

  [Fact]
  public void Create_WithInvalidAmountAndNullCurrency_ChecksAmountFirst()
  {
    var result = Money.Create(0, null!);

    Assert.True(result.IsFailure);
    Assert.Equal(MoneyErrors.NotPositive.Code, result.Error.Code);
    Assert.Equal(MoneyErrors.NotPositive.Message, result.Error.Message);
  }

  [Fact]
  public void TwoMoneyInstances_WithSameAmountAndCurrency_AreEqual()
  {
    var currencyA = CreateUsd();
    var currencyB = CreateUsd();

    var first = Money.Create(2500, currencyA).Value;
    var second = Money.Create(2500, currencyB).Value;

    Assert.Equal(first, second);
  }

  [Fact]
  public void MoneyErrors_HaveExpectedCodesAndMessages()
  {
    Assert.Equal("MoneyErrors.NotPositive", MoneyErrors.NotPositive.Code);
    Assert.Equal("Amount must be greater than zero.", MoneyErrors.NotPositive.Message);

    Assert.Equal("MoneyErrors.CurrencyRequired", MoneyErrors.CurrencyRequired.Code);
    Assert.Equal("Currency type is required.", MoneyErrors.CurrencyRequired.Message);
  }
}