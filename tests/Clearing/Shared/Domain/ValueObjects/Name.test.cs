using OnePlatform.Clearing.Shared.Domain.ValueObjects;

namespace OnePlatform.Clearing.Tests.Shared.Domain.ValueObjects;

public class NameTest
{
  // ---------- Required rule ----------

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void Create_WithNullOrBlankValue_ReturnsRequiredError(string? value)
    {
        // Arrange: the value comes from [InlineData]

        // Act
        var result = Name.Create(value);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(NameErrors.Required, result.Error);
    }

    // ---------- Valid values ----------

    [Fact]
    public void Create_WithValidValue_ReturnsSuccess()
    {
        // Arrange
        var value = "First Bank";

        // Act
        var result = Name.Create(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("First Bank", result.Value.Value);
    }

    [Theory]
    [InlineData("  First Bank", "First Bank")]
    [InlineData("First Bank  ", "First Bank")]
    [InlineData("  First Bank  ", "First Bank")]
    [InlineData("\tFirst Bank\n", "First Bank")]
    public void Create_WithSurroundingWhitespace_TrimsTheValue(string input, string expected)
    {
        // Arrange: input and expected come from [InlineData]

        // Act
        var result = Name.Create(input);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Value.Value);
    }

    [Theory]
    [InlineData("Banco de Bogotá")]
    [InlineData("Caja Social Ñandú")]
    [InlineData("Crédit Agricole")]
    public void Create_WithAccentsOrSpecialLetters_KeepsThem(string value)
    {
        // Arrange: value comes from [InlineData]

        // Act
        var result = Name.Create(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value.Value);
    }
    // ---------- Maximum length rule ----------

    [Fact]
    public void Create_WithExactlyMaxLength_ReturnsSuccess()
    {
        // Arrange
        var value = new string('A', Name.MaxLength);

        // Act
        var result = Name.Create(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(Name.MaxLength, result.Value.Value.Length);
    }

    [Fact]
    public void Create_WithMoreThanMaxLength_ReturnsTooLongError()
    {
        // Arrange
        var value = new string('A', Name.MaxLength + 1);

        // Act
        var result = Name.Create(value);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(NameErrors.TooLong, result.Error);
    }

    [Fact]
    public void Create_WhenOnlyThePaddingMakesItTooLong_ReturnsSuccess()
    {
        // Arrange: MaxLength letters plus extra spaces around them
        var value = "  " + new string('A', Name.MaxLength) + "  ";

        // Act
        var result = Name.Create(value);

        // Assert: after trimming it fits, so it is valid
        Assert.True(result.IsSuccess);
        Assert.Equal(Name.MaxLength, result.Value.Value.Length);
    }
// ---------- Equality and ToString ----------

    [Fact]
    public void TwoNamesWithSameValue_AreEqual()
    {
        // Arrange
        var first = Name.Create("First Bank").Value;
        var second = Name.Create("First Bank").Value;

        // Act + Assert
        Assert.Equal(first, second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void TwoNamesWithDifferentValues_AreNotEqual()
    {
        // Arrange
        var first = Name.Create("First Bank").Value;
        var second = Name.Create("Second Bank").Value;

        // Act + Assert
        Assert.NotEqual(first, second);
    }

    [Fact]
    public void ToString_ReturnsTheValue()
    {
        // Arrange
        var name = Name.Create("First Bank").Value;

        // Act
        var text = name.ToString();

        // Assert
        Assert.Equal("First Bank", text);
    }

}