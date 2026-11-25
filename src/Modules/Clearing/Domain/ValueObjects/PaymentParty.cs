using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Clearing.Domain.ValueObjects;

public sealed record PaymentParty
{
  public string AccountNumber { get; } = string.Empty;
  public string Name { get; } = string.Empty;
  public Guid InstitutionId { get; }

  private PaymentParty() {}
  private PaymentParty(
    string accountNumber,
    string name,
    Guid institutionId)
  {
    AccountNumber = accountNumber;
    Name = name;
    InstitutionId = institutionId;
  }

  public static Result<PaymentParty> Create(string accountNumber, string name, Guid institutionId)
  {
    if (string.IsNullOrWhiteSpace(accountNumber))
      return Result.Failure<PaymentParty>(PartyDetailsErrors.AccountRequired);

    if (string.IsNullOrWhiteSpace(name))
      return Result.Failure<PaymentParty>(PartyDetailsErrors.NameRequired);
    
    if (institutionId == Guid.Empty)
      return Result.Failure<PaymentParty>(PartyDetailsErrors.InstitutionIDRequired);

    return Result.Success(new PaymentParty(accountNumber.Trim(), name.Trim(), institutionId));
  }
}

public static class PartyDetailsErrors
{
  public static readonly Error AccountRequired = new(
    "PaymentParty.AccountRequired", "Member account is required.");

  public static readonly Error NameRequired = new(
    "PaymentParty.NameRequired", "Member name is required.");

  public static readonly Error InstitutionIDRequired = new(
    "PaymentParty.InstitutionIDRequired", "InstitutionId is required.");
}