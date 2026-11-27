using OnePlatform.Clearing.Shared.Domain.Exceptions;

namespace OnePlatform.Clearing.Shared.Domain.ValueObjects;

public sealed record Name
{
    public const int MaxLength = 200;
    public string Value { get; }
    private Name(string value) => Value = value;

    public static Result<Name> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Name>(NameErrors.Required);

        var trimmed = value.Trim();

        if (trimmed.Length > MaxLength)
            return Result.Failure<Name>(NameErrors.TooLong);

        return Result.Success(new Name(trimmed));
    }

    public override string ToString() => Value;
}

public static class NameErrors
{
    public static readonly Error Required = new(
        "Name.Required", "Name is required.");

    public static readonly Error TooLong = new(
        "Name.TooLong", $"Name cannot exceed {Name.MaxLength} characters.");
}