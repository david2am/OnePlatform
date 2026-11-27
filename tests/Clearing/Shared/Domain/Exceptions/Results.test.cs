
using OnePlatform.Clearing.Shared.Domain.Exceptions;

namespace OnePlatform.Clearing.Tests.Shared.Domain.Exceptions;

public sealed class ResultTests
{
    [Fact]
    public void Success_ShouldReturnSuccessfulResult()
    {
        // Act
        var result = Result.Success();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);
    }

    [Fact]
    public void Failure_ShouldReturnFailedResult()
    {
        // Arrange
        var error = new Error(
            "Payment.Invalid",
            "The payment is invalid.",
            ErrorType.Validation
        );

        // Act
        var result = Result.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenSuccessHasError()
    {
        // Arrange
        var error = new Error("Payment.Invalid", "Invalid payment.");

        // Act
        var action = () => new TestResult(true, error);

        // Assert
        Assert.Throws<InvalidOperationException>(action);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenFailureHasNoError()
    {
        // Act
        var action = () => new TestResult(false, Error.None);

        // Assert
        Assert.Throws<InvalidOperationException>(action);
    }

    // Test-only derived class
    private sealed class TestResult(bool isSuccess, Error error)
        : Result(isSuccess, error);
}
