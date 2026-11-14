using Microsoft.EntityFrameworkCore;
using OnePlatform.Clearing.Domain.Entities;
using OnePlatform.Clearing.Domain.ValueObjects;
using OnePlatform.Clearing.Infrastructure.Persistence.SqlServer.Configurations;

namespace OnePlatform.Clearing.Infrastructure.Persistence.SqlServer;

public class ClearingDbContext : DbContext
{
  public ClearingDbContext(
    DbContextOptions<ClearingDbContext> options
  ) : base (options)
  {}

  public DbSet<Payment> Payments => Set<Payment>();
  public DbSet<Institution> Institutions => Set<Institution>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(
      typeof(ClearingDbContext).Assembly
    );
  }

  protected override void ConfigureConventions(
    ModelConfigurationBuilder configurationBuilder
  )
  {
    configurationBuilder.Properties<PaymentId>()
      .HaveConversion<GuidIdConverter<PaymentId>>();

    configurationBuilder.Properties<InstitutionId>()
      .HaveConversion<GuidIdConverter<InstitutionId>>();
  }
}