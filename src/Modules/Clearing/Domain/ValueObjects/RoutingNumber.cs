using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Clearing.Domain.ValueObjects;

public sealed record RoutingNumber
{
  private const int Length = 9;

  public string Value { get; } = string.Empty;

  private RoutingNumber() {}
  private RoutingNumber(string value)
  {
    Value = value;
  }

  public static Result<RoutingNumber> Create(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return Result.Failure<RoutingNumber>(RoutingNumberErrors.Required);

    if (!IsValidRoutingNumber(value))
      return Result.Failure<RoutingNumber>(RoutingNumberErrors.Invalid);
  
    return Result.Success<RoutingNumber>(new(value.Trim()));
  }

  // ABA checksum: 3*(d1+d4+d7) + 7*(d2+d5+d8) + 1*(d3+d6+d9) must be divisible by 10
  private static bool IsValidRoutingNumber(string? value)
  {
    if (value is null || value.Length != 9 || !value.All(char.IsAsciiDigit))
      return false;

    var d = value.Select(c => c - '0').ToArray();

    var sum = 3 * (d[0] + d[3] + d[6])
            + 7 * (d[1] + d[4] + d[7])
            + 1 * (d[2] + d[5] + d[8]);

    return sum % 10 == 0;
  }
}

public static class RoutingNumberErrors
{
    public static readonly Error Required = new(
    "RoutingNumber.Required",
    "Routing number is required");
    public static readonly Error Invalid = new(
    "RoutingNumber.Invalid",
    "Routing number must be 9 digits and pass the ABA checksum.");
}