namespace OnePlatform.Clearing.Domain.Shared;

public enum ErrorType { Failure, Validation, NotFound, Conflict }
public sealed record Error(string Code, string Description, ErrorType Type = ErrorType.Failure)
{
  public static readonly Error None = new (string.Empty, string.Empty);
  public static readonly Error NullValue = new ("Error.NullValue", "The specified result value is null.");
  
}