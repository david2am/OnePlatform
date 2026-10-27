namespace OnePlatform.Clearing.Domain.Shared;

public sealed record Error(string Code, string Description)
{
  public static readonly Error None = new Error(string.Empty, string.Empty);
  public static readonly Error NullValue = new Error("Error.NullValue", "The specified result value is unll.");
  
}