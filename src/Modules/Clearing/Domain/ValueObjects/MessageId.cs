using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Clearing.Domain.ValueObjects;

public sealed record MessageId
{
  public const int MaxLength = 35; // ISO 20022 Max35Text

  public string Value { get; } = string.Empty;

  private MessageId() {}
  private MessageId(string value)
  {
    Value = value;
  }

  public static Result<MessageId> Create(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return Result.Failure<MessageId>(MessageIdErrors.Required);

    if (value.Length > MaxLength)
      return Result.Failure<MessageId>(MessageIdErrors.TooLong);

    return Result.Success(new MessageId(value));
  }

  public override string ToString() => Value;
}

public static class MessageIdErrors
{
    public static readonly Error Required = new(
    "MessageId.Required",
    "Message Id id is required."
  );
  
  public static readonly Error TooLong = new(
    "MessageId.TooLong",
    "Message Id is too long."
  );
}