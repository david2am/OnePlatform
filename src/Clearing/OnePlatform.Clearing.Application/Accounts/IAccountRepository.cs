using OnePlatform.Clearing.Domain.Entities;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Application.Accounts;

public interface IAccountRepository
{
  Task AddAsync(
    Account account,
    CancellationToken ct = default
  );

  Task<IReadOnlyList<Account>> GetByInstitutionIdAsync(
    InstitutionId institutionId,
    CancellationToken ct = default
  );
}