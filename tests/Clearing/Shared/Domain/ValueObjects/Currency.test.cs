using OnePlatform.Clearing.Shared.Domain.ValueObjects;

namespace OnePlatform.Clearing.Shared.Domain.Tests.ValueObjects;

public class CurrencyTests
{
  // ---------- 1. VALID INPUT ----------

  [Theory]
  [InlineData("USD", "USD")]   // already correct
  [InlineData("eur", "EUR")]   // lowercase -> uppercase
  [InlineData("cOp", "COP")]   // mixed case -> uppercase
  [InlineData("  GBP  ", "GBP")] // spaces around -> trimmed
  [InlineData("\tjpy\n", "JPY")] // tabs / newlines -> trimmed
  public void Create_WithValidCode_ReturnsSuccessAndNormalizedCode(
    string input,
    string expectedCode)
  {
    var result = Currency.Create(input);

    Assert.True(result.IsSuccess);
    Assert.Equal(expectedCode, result.Value.Code);
  }

  [Fact]
  public void Create_WithUnknownButWellFormedCode_ReturnsSuccess()
  {
    // Documents current behavior: only the FORMAT is validated
    // (3 letters A-Z), not whether the code exists in ISO 4217.
    var result = Currency.Create("ZZZ");

    Assert.True(result.IsSuccess);
    Assert.Equal("ZZZ", result.Value.Code);
  }

  // ---------- 2. INVALID INPUT ----------

  [Theory]
  [InlineData(null)]      // null
  [InlineData("")]        // empty
  [InlineData("   ")]     // only whitespace
  public void Create_WithNullOrBlank_ReturnsInvalidCodeFailure(string? input)
  {
    var result = Currency.Create(input!);

    Assert.True(result.IsFailure);
    Assert.Equal(CurrencyErrors.InvalidCode, result.Error);
  }

  [Theory]
  [InlineData("U")]       // too short
  [InlineData("US")]      // too short
  [InlineData("USDD")]    // too long
  [InlineData("US D")]    // inner space makes it 4 chars
  [InlineData(" US ")]    // trims to "US" (2 chars)
  public void Create_WithWrongLength_ReturnsInvalidCodeFailure(string input)
  {
    var result = Currency.Create(input);

    Assert.True(result.IsFailure);
    Assert.Equal(CurrencyErrors.InvalidCode, result.Error);
  }

  [Theory]
  [InlineData("123")]     // digits only
  [InlineData("US1")]     // letters + digit
  [InlineData("U$D")]     // symbol
  [InlineData("U D")]     // inner space, 3 chars
  [InlineData("U-D")]     // hyphen
  [InlineData("ÉUR")]     // accented letter (not A-Z)
  [InlineData("éur")]     // accented lowercase, becomes "ÉUR"
  public void Create_WithNonAsciiLetters_ReturnsInvalidCodeFailure(string input)
  {
    var result = Currency.Create(input);

    Assert.True(result.IsFailure);
    Assert.Equal(CurrencyErrors.InvalidCode, result.Error);
  }

  // ---------- 3. EQUALITY (record behavior) ----------

  [Fact]
  public void Create_WithSameCodeInDifferentFormats_ProducesEqualCurrencies()
  {
    var a = Currency.Create("usd").Value;
    var b = Currency.Create("  USD ").Value;

    Assert.Equal(a, b);
    Assert.Equal(a.GetHashCode(), b.GetHashCode());
  }

  [Fact]
  public void Create_WithDifferentCodes_ProducesDifferentCurrencies()
  {
    var usd = Currency.Create("USD").Value;
    var eur = Currency.Create("EUR").Value;

    Assert.NotEqual(usd, eur);
  }

  // ---------- 4. ERROR DEFINITION ----------

  [Fact]
  public void InvalidCodeError_HasExpectedCodeAndMessage()
  {
    Assert.Equal("CurrencyErrors.InvalidCode", CurrencyErrors.InvalidCode.Code);
    Assert.Equal(
      "Currency must be a 3-letter ISO code, for example USD.",
      CurrencyErrors.InvalidCode.Message);
  }
}