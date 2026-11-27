using OnePlatform.Clearing.Shared.Domain.Exceptions;

namespace OnePlatform.Clearing.Shared.Domain.ValueObjects;

public sealed record Name
{
    public string Value { get; }

    private Name(string value) => Value = value;

    public static Result<Name> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Name>(NameErrors.Required);

        return Result.Success(new Name(value.Trim()));
    }

    public override string ToString() => Value;
}

public static class NameErrors
{
    public static readonly Error Required = new(
        "Name.Required", "Name is required.");
}