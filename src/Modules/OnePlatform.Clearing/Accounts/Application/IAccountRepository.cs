using OnePlatform.Clearing.Accounts.Domain.Entities;
using OnePlatform.Clearing.Accounts.Domain.ValueObjects;
using OnePlatform.Clearing.Institutions.Domain.ValueObjects;

namespace OnePlatform.Clearing.Accounts.Application;

public interface IAccountRepository
{
  Task AddAsync(
    Account account,
    CancellationToken ct = default
  );

  Task<Account?> GetByIdAsync(
    AccountId Id,
    CancellationToken ct = default
  );

  Task<IReadOnlyList<Account>> GetByInstitutionIdAsync(
    InstitutionId institutionId,
    CancellationToken ct = default
  );
}