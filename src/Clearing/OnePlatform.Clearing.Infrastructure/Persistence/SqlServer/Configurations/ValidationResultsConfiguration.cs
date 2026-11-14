using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnePlatform.Clearing.Domain.Entities;

namespace OnePlatform.Clearing.Infrastructure.Persistence.SqlServer.Configurations;

public sealed class ValidationResultsConfiguration
  : IEntityTypeConfiguration<ValidationResult>
{
  public void Configure(EntityTypeBuilder<ValidationResult> builder)
  {
    builder.ToTable("ValidationResults");
    builder.HasKey(p => p.Id);
  }
}