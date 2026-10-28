using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Clearing.Domain.ValueObjects;

public sealed record PartyDetails
{
  public string AccountNumber { get; }
  public string Name { get; }
  public Guid InstitutionId { get; }
  private PartyDetails(
    string accountNumber,
    string name,
    Guid institutionId)
  {
    AccountNumber = accountNumber;
    Name = name;
    InstitutionId = institutionId;
  }

  public static Result<PartyDetails> Create(string accountNumber, string name, Guid institutionId)
  {
    if (string.IsNullOrWhiteSpace(accountNumber))
      return Result.Failure<PartyDetails>(PartyDetailsErrors.AccountRequired);

    if (string.IsNullOrWhiteSpace(name))
      return Result.Failure<PartyDetails>(PartyDetailsErrors.NameRequired);
    
    if (institutionId == Guid.Empty)
      return Result.Failure<PartyDetails>(PartyDetailsErrors.InstitutionIDRequired);

    return Result.Success(new PartyDetails(accountNumber.Trim(), name.Trim(), institutionId));
  }
}

public static class PartyDetailsErrors
{
  public static readonly Error AccountRequired = new(
    "PartyDetails.AccountRequired", "Member account is required.");

  public static readonly Error NameRequired = new(
    "PartyDetails.NameRequired", "Member name is required.");

  public static readonly Error InstitutionIDRequired = new(
    "PartyDetails.InstitutionIDRequired", "InstitutionId is required.");
}