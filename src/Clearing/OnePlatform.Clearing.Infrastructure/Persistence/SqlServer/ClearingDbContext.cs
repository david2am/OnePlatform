using Microsoft.EntityFrameworkCore;
using OnePlatform.Clearing.Domain.Entities;

namespace OnePlatform.Clearing.Infrastructure.Persistence.SqlServer;

public class ClearingDbContext : DbContext
{
  public ClearingDbContext(DbContextOptions<ClearingDbContext> options)
    : base (options)
  {}

  public DbSet<Payment> Payments => Set<Payment>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClearingDbContext).Assembly);
  }
}