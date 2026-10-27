using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Clearing.Domain.ValueObjects;

public sealed record PartyDetails
{
  private PartyDetails(string accountNumber, string name)
  {
    AccountNumber = accountNumber;
    Name = name;
  }

  public string AccountNumber { get; }
  public string Name { get; }

  public static Result<PartyDetails> Create(string accountNumber, string name)
  {
    if (string.IsNullOrWhiteSpace(accountNumber))
      return Result.Failure<PartyDetails>(PartyDetailsErrors.AccountRequired);

    if (string.IsNullOrWhiteSpace(name))
      return Result.Failure<PartyDetails>(PartyDetailsErrors.NameRequired);

    return Result.Success(new PartyDetails(accountNumber.Trim(), name.Trim()));
  }
}

public static class PartyDetailsErrors
{
  public static readonly Error AccountRequired = new(
    "PartyDetails.AccountRequired", "Member account is required.");

  public static readonly Error NameRequired = new(
    "PartyDetails.NameRequired", "Member name is required.");
}