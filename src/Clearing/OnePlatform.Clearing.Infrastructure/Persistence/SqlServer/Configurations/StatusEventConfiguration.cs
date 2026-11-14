using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnePlatform.Clearing.Domain.Entities;

namespace OnePlatform.Clearing.Infrastructure.Persistence.SqlServer.Configurations;

public sealed class StatusEventConfiguration
  : IEntityTypeConfiguration<StatusEvent>
{
  public void Configure(EntityTypeBuilder<StatusEvent> builder)
  {
    builder.ToTable("StatusEvents");
    builder.HasKey(p => p.Id);
  }
}