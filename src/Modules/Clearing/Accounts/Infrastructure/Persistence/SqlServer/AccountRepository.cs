using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using OnePlatform.Clearing.Accounts.Application;
using OnePlatform.Clearing.Accounts.Domain.Entities;
using OnePlatform.Clearing.Accounts.Domain.ValueObjects;
using OnePlatform.Clearing.Institutions.Domain.ValueObjects;
using OnePlatform.Clearing.Shared.Infrastructure.Persistence.SqlServer;

namespace OnePlatform.Clearing.Accounts.Infrastructure.Persistence.SqlServer;

internal class AccountRepository : IAccountRepository
{
  private readonly ClearingDbContext _context;

  public AccountRepository(ClearingDbContext context)
  {
    _context = context;
  }

  public async Task AddAsync(Account account, CancellationToken ct)
  {
    await _context.Accounts.AddAsync(account, ct);
    await _context.SaveChangesAsync(ct);
  }

  public async Task<Account?> GetByIdAsync(
    AccountId id,
    CancellationToken ct
  ) => await _context.Accounts
        .AsNoTracking()
        .FirstOrDefaultAsync(a => a.Id == id, ct);

  public async Task<IReadOnlyList<Account>> GetByInstitutionIdAsync(
    InstitutionId institutionId,
    CancellationToken ct
  ) =>
    await _context.Accounts
      .AsNoTracking()
      .Where(a => a.InstitutionId == institutionId)
      .OrderBy(a => a.Currency)
      .ThenBy(a => a.AccountNumber)
      .ToListAsync(ct);
}