using OnePlatform.Clearing.Domain.Enums;
using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Clearing.Domain.Entities;

public sealed class Account : AggregateRoot
{
  private Account(Guid id) : base(id){}

  private Account(
    Guid id,
    Guid institutionId,
    string accountNumber,
    string currency,
    DateTimeOffset openedAt
  ) : base(id)
  {
    InstitutionId = institutionId;
    AccountNumber = accountNumber;
    Currency = currency;
    OpenedAt = openedAt;
    Status = AccountStatus.Active;
  }

  public Guid InstitutionId { get; private set; }
  public string AccountNumber { get; private set; } = string.Empty;
  public string Currency { get; private set; } = string.Empty;
  public DateTimeOffset OpenedAt { get; private set; }
  public AccountStatus Status { get; private set; }

  public static Result<Account> Open(
    Guid institutionId,
    string accountNumber,
    string currency,
    DateTimeOffset openedAt)
  {
    if (institutionId == Guid.Empty)
      return Result.Failure<Account>(AccountErrors.InstitutionRequired);

    if (!IsValidAccountNumber(accountNumber))
      return Result.Failure<Account>(AccountErrors.InvalidAccountNumber);

    var normalizedCurency = currency?.Trim().ToUpperInvariant() ?? string.Empty;

    if (!IsValidCurrency(normalizedCurency))
      return Result.Failure<Account>(AccountErrors.InvalidCurrency);

    return Result.Success(
      new Account(
        Guid.CreateVersion7(),
        institutionId,
        accountNumber,
        normalizedCurency,
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

  private static bool IsValidCurrency(string value)
    => value.Length == 3 && value.All(char.IsAsciiLetterUpper);
}

public static class AccountErrors
{
  public static readonly Error InstitutionRequired = new(
    "Account.InstitutionRequired", "Institution id is required.");

  public static readonly Error InvalidAccountNumber = new(
    "Account.InvalidAccountNumber",
    "Account number must contain 1 to 17 digits.");

  public static readonly Error InvalidCurrency = new(
    "Account.InvalidCurrency",
    "Currency must be a 3-letter ISO code, for example USD.");

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