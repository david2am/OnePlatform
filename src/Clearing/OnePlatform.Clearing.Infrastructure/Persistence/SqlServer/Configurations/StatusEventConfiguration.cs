
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

        builder.HasKey(e => e.Id);

        builder.Property(e => e.PaymentId)
               .IsRequired();

        builder.Property(e => e.EventType)
               .HasConversion<string>()
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(e => e.OccurredAt)
               .IsRequired();

        builder.Property(e => e.Details)
               .HasMaxLength(1000)
               .IsRequired(false);

        builder.HasIndex(e => new
               {
                   e.PaymentId,
                   e.OccurredAt
               });
    }
}
