using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Clearing.Domain.ValueObjects;

public sealed record EndToEndId
{
  public const int MaxLength = 35; // ISO 20022 Max35Text

  public string Value { get; } = string.Empty;

  private EndToEndId() {}
  private EndToEndId(string value)
  {
    Value = value;
  }

  public static Result<EndToEndId> Create(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return Result.Failure<EndToEndId>(EndToEndIdErrors.Required);

    if (value.Length > MaxLength)
      return Result.Failure<EndToEndId>(EndToEndIdErrors.TooLong);

    return Result.Success(new EndToEndId(value));
  }

  public override string ToString() => Value;
}

public static class EndToEndIdErrors
{
    public static readonly Error Required = new(
    "EndToEnd.Required",
    "End-to-end id is required."
  );
  
  public static readonly Error TooLong = new(
    "EndToEnd.TooLong",
    "End-to-end id is too long."
  );
}