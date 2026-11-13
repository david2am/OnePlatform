using Microsoft.EntityFrameworkCore;
using OnePlatform.Clearing.Application.Institutions;
using OnePlatform.Clearing.Domain.Entities;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Infrastructure.Persistence.SqlServer;

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

  public async Task<Institution?> GetByRoutingNumberAsync(
    RoutingNumber routingNumber,
    CancellationToken ct
  ) =>
    await _context.Institutions
        .FirstOrDefaultAsync(i => i.RoutingNumber == routingNumber);
}