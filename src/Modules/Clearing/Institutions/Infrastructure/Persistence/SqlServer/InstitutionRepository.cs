using Microsoft.EntityFrameworkCore;
using OnePlatform.Clearing.Institutions.Application;
using OnePlatform.Clearing.Institutions.Domain.Entities;
using OnePlatform.Clearing.Institutions.Domain.ValueObjects;
using OnePlatform.Clearing.Shared.Infrastructure.Persistence.SqlServer;

namespace OnePlatform.Clearing.Institutions.Infrastructure.Persistence.SqlServer;

public sealed class InstitutionRepository : IInstitutionRepository
{
  private readonly ClearingDbContext _context;

  public InstitutionRepository(ClearingDbContext context)
  {
    _context = context;
  }

  public async Task AddAsync(
    Institution institution,
    CancellationToken ct
  )
  {
    await _context.AddAsync(institution, ct);  
    await _context.SaveChangesAsync();
  }

  public async Task<Institution?> GetByIdAsync(
    InstitutionId id,
    CancellationToken ct
  ) =>
    await _context.Institutions
        .FirstOrDefaultAsync(i => i.Id == id);

  public async Task<Institution?> GetByRoutingNumberAsync(
    RoutingNumber routingNumber,
    CancellationToken ct
  ) =>
    await _context.Institutions
        .FirstOrDefaultAsync(i => i.RoutingNumber == routingNumber);
}