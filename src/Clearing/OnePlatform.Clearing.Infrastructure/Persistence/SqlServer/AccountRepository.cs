using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using OnePlatform.Clearing.Application.Accounts;
using OnePlatform.Clearing.Domain.Entities;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Infrastructure.Persistence.SqlServer;

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

  public async Task<Account?> GetByInstitutionIdAsync(
    InstitutionId institutionId,
    CancellationToken ct
  ) =>
    await _context.Accounts
        .FirstOrDefaultAsync(
          a => a.InstitutionId == institutionId,
          ct
        );
}