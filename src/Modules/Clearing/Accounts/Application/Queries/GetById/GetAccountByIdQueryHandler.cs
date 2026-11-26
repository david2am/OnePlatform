

using OnePlatform.Clearing.Accounts.Domain.ValueObjects;
using OnePlatform.Clearing.Shared.Application.Abstractions;
using OnePlatform.Clearing.Shared.Domain.Exceptions;

namespace OnePlatform.Clearing.Accounts.Application.Queries;

public sealed class GetAccountsByIdQueryHandler
  : IQueryHandler<GetAccountsByIdQuery, AccountResponse>
{
  private readonly IAccountRepository _accounts;

  public GetAccountsByIdQueryHandler(
    IAccountRepository accounts
  )
  {
    _accounts = accounts;
  }

  public async Task<Result<AccountResponse>> Handle(
    GetAccountsByIdQuery request,
    CancellationToken ct
  )
  {
    var idResult = AccountId.Create(request.Id);
    if (idResult.IsFailure)
      return Result.Failure<AccountResponse>(idResult.Error);

    var account = await _accounts.GetByIdAsync(
      idResult.Value, ct
    );

    if (account is null)
      return Result.Failure<AccountResponse>(
        new Error(
          "Account.NotFound",
          $"The Account with Id {request.Id} was not found"
        )
      );

    var response = new AccountResponse(
      account.Id,
      account.InstitutionId,
      account.AccountNumber,
      account.Currency,
      account.Status,
      account.OpenedAt
    );
    return Result.Success(response);
  }
}