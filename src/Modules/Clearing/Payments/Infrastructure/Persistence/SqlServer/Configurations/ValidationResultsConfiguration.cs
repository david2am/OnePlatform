
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnePlatform.Clearing.Payments.Domain.Entities;

namespace OnePlatform.Clearing.Payments.Infrastructure.Persistence.SqlServer.Configurations;

public sealed class ValidationResultConfiguration
    : IEntityTypeConfiguration<ValidationResult>
{
    public void Configure(EntityTypeBuilder<ValidationResult> builder)
    {
        builder.ToTable("ValidationResults");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.PaymentId)
               .IsRequired();

        // CHANGED: Explicit enum conversion.
        builder.Property(v => v.CheckType)
               .HasConversion<string>()
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(v => v.Outcome)
               .HasConversion<string>()
               .HasMaxLength(10)
               .IsRequired();

        builder.Property(v => v.CheckedAt)
               .IsRequired();

        builder.HasIndex(v => new
               {
                   v.PaymentId,
                   v.CheckedAt
               });
    }
}
