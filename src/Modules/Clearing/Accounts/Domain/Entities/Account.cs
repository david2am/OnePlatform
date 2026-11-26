using OnePlatform.Clearing.Accounts.Domain.Enums;
using OnePlatform.Clearing.Accounts.Domain.ValueObjects;
using OnePlatform.Clearing.Institutions.Domain.ValueObjects;
using OnePlatform.Clearing.Shared.Domain.Entities;
using OnePlatform.Clearing.Shared.Domain.Exceptions;
using OnePlatform.Clearing.Shared.Domain.ValueObjects;

namespace OnePlatform.Clearing.Accounts.Domain.Entities;

public sealed class Account : AggregateRoot<AccountId>
{
  public InstitutionId InstitutionId { get; private set; }
  public string AccountNumber { get; private set; } = string.Empty;
  public Currency Currency { get; private set; }
  public DateTimeOffset OpenedAt { get; private set; }
  public AccountStatus Status { get; private set; }

  private Account() : base(null!)
  {
    InstitutionId = null!;
    Currency = null!;
  }

  private Account(
    AccountId id,
    InstitutionId institutionId,
    string accountNumber,
    Currency currency,
    DateTimeOffset openedAt
  ) : base(id)
  {
    InstitutionId = institutionId;
    AccountNumber = accountNumber;
    Currency = currency;
    OpenedAt = openedAt;
    Status = AccountStatus.Active;
  }

  public static Result<Account> Open(
    InstitutionId institutionId,
    string accountNumber,
    Currency currency,
    DateTimeOffset openedAt)
  {
    if (institutionId is null)
      return Result.Failure<Account>(AccountErrors.InstitutionRequired);

    if (!IsValidAccountNumber(accountNumber))
      return Result.Failure<Account>(AccountErrors.InvalidAccountNumber);
    
    if (currency is null)
      return Result.Failure<Account>(AccountErrors.CurrencyRequired);

    return Result.Success(
      new Account(
        AccountId.New(),
        institutionId,
        accountNumber,
        currency,
        openedAt
      )
    );
  }

  public Result Freeze()
  {
    if (Status == AccountStatus.Closed)
      return Result.Failure(AccountErrors.AccountClosed);

    if (Status == AccountStatus.Frozen)
      return Result.Failure(AccountErrors.AlreadyFrozen);

    Status = AccountStatus.Frozen;
    return Result.Success();
  }

  public Result Unfreeze()
  {
    if (Status == AccountStatus.Closed)
      return Result.Failure(AccountErrors.AccountClosed);

    if (Status != AccountStatus.Frozen)
      return Result.Failure(AccountErrors.NotFrozen);

    Status = AccountStatus.Active;
    return Result.Success();
  }

  // The balance is derived from ledger entries, so the caller
  // must calculate it and pass it in.
  public Result Close(long currentBalanceMinor)
  {
    if (Status == AccountStatus.Closed)
      return Result.Failure(AccountErrors.AccountClosed);

    if (currentBalanceMinor != 0)
      return Result.Failure(AccountErrors.BalanceNotZero);

    Status = AccountStatus.Closed;
    return Result.Success();
  }

  private static bool IsValidAccountNumber(string? value)
  => !string.IsNullOrEmpty(value)
      && value.Length <= 17
      && value.All(char.IsAsciiDigit);
}

public static class AccountErrors
{
  public static readonly Error InstitutionRequired = new(
    "Account.InstitutionRequired", "Institution id is required.");

  public static readonly Error InvalidAccountNumber = new(
    "Account.InvalidAccountNumber",
    "Account number must contain 1 to 17 digits.");
  
  public static readonly Error CurrencyRequired = new(
    "Account.CurrencyRequired",
    "A currency should be provided.");

  public static readonly Error AccountClosed = new(
    "Account.AccountClosed", "The account is closed.");

  public static readonly Error AlreadyFrozen = new(
    "Account.AlreadyFrozen", "The account is already frozen.");

  public static readonly Error NotFrozen = new(
    "Account.NotFrozen", "Only a frozen account can be unfrozen.");

  public static readonly Error BalanceNotZero = new(
    "Account.BalanceNotZero",
    "An account can only be closed when its balance is zero.");
}