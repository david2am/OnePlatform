using OnePlatform.Clearing.Institutions.Domain.ValueObjects;
using OnePlatform.Clearing.Shared.Application.Abstractions;
using OnePlatform.Clearing.Shared.Domain.Exceptions;

namespace OnePlatform.Clearing.Accounts.Application.Queries;

public sealed class GetAccountsByInstitutionIdQueryHandler
  : IQueryHandler<
      GetAccountsByInstitutionIdQuery,
      IReadOnlyList<AccountResponse>
    >
{
  private readonly IAccountRepository _accounts;

  public GetAccountsByInstitutionIdQueryHandler(
    IAccountRepository accounts
  )
  {
    _accounts = accounts;
  }

  public async Task<Result<IReadOnlyList<AccountResponse>>> Handle(
    GetAccountsByInstitutionIdQuery request,
    CancellationToken ct
  )
  {
    var institutionIdResult = InstitutionId.Create(request.InstitutionId);
    if (institutionIdResult.IsFailure)
      return Result.Failure<IReadOnlyList<AccountResponse>>(institutionIdResult.Error);

    var accounts = await _accounts.GetByInstitutionIdAsync(
      institutionIdResult.Value, ct
    );

    var response = accounts
      .Select(a => new AccountResponse(
        a.Id,
        a.InstitutionId,
        a.AccountNumber,
        a.Currency,
        a.Status,
        a.OpenedAt))
      .ToList(); 

    return Result.Success<IReadOnlyList<AccountResponse>>(response);
  }
}