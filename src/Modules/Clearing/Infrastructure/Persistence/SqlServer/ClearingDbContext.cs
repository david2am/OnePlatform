using Microsoft.EntityFrameworkCore;
using OnePlatform.Clearing.Domain.Entities;
using OnePlatform.Clearing.Domain.ValueObjects;
using OnePlatform.Clearing.Infrastructure.Persistence.SqlServer.Converters;

namespace OnePlatform.Clearing.Infrastructure.Persistence.SqlServer;

public class ClearingDbContext : DbContext
{
  public ClearingDbContext(
    DbContextOptions<ClearingDbContext> options
  ) : base (options)
  {}

  public DbSet<Payment> Payments => Set<Payment>();
  public DbSet<Institution> Institutions => Set<Institution>();
  public DbSet<Account> Accounts => Set<Account>();
  public DbSet<Participant> Participants => Set<Participant>();

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

    configurationBuilder.Properties<AccountId>()
      .HaveConversion<GuidIdConverter<AccountId>>();
    
    configurationBuilder.Properties<ParticipantId>()
      .HaveConversion<GuidIdConverter<ParticipantId>>();

    configurationBuilder.Properties<Currency>()
      .HaveConversion<CurrencyConverter>()
      .HaveColumnType("char(3)");
  }
}