using OnePlatform.Clearing.Shared.Domain.Exceptions;

namespace OnePlatform.Clearing.Tests.Shared.Domain.Exceptions;

public class ErrorTests
{
    [Fact]
    public void Constructor_ShouldCreateError_WithProvidedValues()
    {
        // Arrange
        const string code = "Payment.InvalidAmount";
        const string message = "The payment amount is invalid.";

        // Act
        var error = new Error(code, message, ErrorType.Validation);

        // Assert
        Assert.Equal(code, error.Code);
        Assert.Equal(message, error.Message);
        Assert.Equal(ErrorType.Validation, error.Type);
    }

    [Fact]
    public void Constructor_ShouldUseFailure_AsDefaultType()
    {
        // Arrange & Act
        var error = new Error(
            "Payment.Failed",
            "The payment operation failed."
        );

        // Assert
        Assert.Equal(ErrorType.Failure, error.Type);
    }

    [Fact]
    public void Errors_WithSameValues_ShouldBeEqual()
    {
        // Arrange
        var first = new Error(
            "Payment.NotFound",
            "Payment was not found.",
            ErrorType.NotFound
        );

        var second = new Error(
            "Payment.NotFound",
            "Payment was not found.",
            ErrorType.NotFound
        );

        // Act & Assert
        Assert.Equal(first, second);
    }

    [Fact]
    public void Errors_WithDifferentCodes_ShouldNotBeEqual()
    {
        // Arrange
        var first = new Error("Payment.NotFound", "Payment was not found.");
        var second = new Error("Account.NotFound", "Payment was not found.");

        // Act & Assert
        Assert.NotEqual(first, second);
    }

    [Fact]
    public void Errors_WithDifferentMessages_ShouldNotBeEqual()
    {
        // Arrange
        var first = new Error("Payment.Invalid", "Invalid payment.");
        var second = new Error("Payment.Invalid", "Invalid amount.");

        // Act & Assert
        Assert.NotEqual(first, second);
    }

    [Fact]
    public void Errors_WithDifferentTypes_ShouldNotBeEqual()
    {
        // Arrange
        var first = new Error("Payment.Invalid", "Invalid payment.", ErrorType.Failure);
        var second = new Error("Payment.Invalid", "Invalid payment.", ErrorType.Validation);

        // Act & Assert
        Assert.NotEqual(first, second);
    }

    [Fact]
    public void None_ShouldHaveEmptyCodeAndMessage()
    {
        // Act
        var error = Error.None;

        // Assert
        Assert.Equal(string.Empty, error.Code);
        Assert.Equal(string.Empty, error.Message);
        Assert.Equal(ErrorType.Failure, error.Type);
    }

    [Fact]
    public void NullValue_ShouldHaveExpectedValues()
    {
        // Act
        var error = Error.NullValue;

        // Assert
        Assert.Equal("Error.NullValue", error.Code);
        Assert.Equal(
            "The specified result value is null.",
            error.Message
        );
        Assert.Equal(ErrorType.Failure, error.Type);
    }

  [Fact]
  public void ErrorType_ShouldHaveExpectedValues()
  {
      Assert.Equal(0, (int)ErrorType.Failure);
      Assert.Equal(1, (int)ErrorType.Validation);
      Assert.Equal(2, (int)ErrorType.NotFound);
      Assert.Equal(3, (int)ErrorType.Conflict);
  }
}
