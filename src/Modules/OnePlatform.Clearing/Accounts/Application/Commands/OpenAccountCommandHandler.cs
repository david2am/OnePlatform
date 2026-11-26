using OnePlatform.Clearing.Accounts.Application;
using OnePlatform.Clearing.Accounts.Domain.Entities;
using OnePlatform.Clearing.Shared.Application.Abstractions;
using OnePlatform.Clearing.Shared.Domain.Exceptions;

namespace OnePlatform.Clearing.Accounts.Application.Commands;

public sealed class OpenAccountCommandHandler
  : ICommandHandler<OpenAccountCommand, Account>
{
  private readonly IAccountRepository _accounts;
  private readonly TimeProvider _timeProvider;

  public OpenAccountCommandHandler(
    IAccountRepository accountRepository,
    TimeProvider timeProvider
  )
  {
    _accounts = accountRepository;
    _timeProvider = timeProvider;
  }

  public async Task<Result<Account>> Handle(
    OpenAccountCommand command,
    CancellationToken ct = default
  )
  {
    var openedAt = _timeProvider.GetUtcNow();

    var result = Account.Open(
      command.InstitutionId,
      command.AccountNumber,
      command.Currency,
      openedAt
    );

    if (result.IsFailure)
      return Result.Failure<Account>(result.Error);

    var account = result.Value;

    await _accounts.AddAsync(account, ct);

    return Result.Success(account);
  }
}