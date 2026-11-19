
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnePlatform.Clearing.Domain.Entities;

namespace OnePlatform.Clearing.Infrastructure.Persistence.SqlServer.Configurations;

public sealed class AccountConfiguration
    : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(a => a.Id);

        // foreign key relationship
        builder.HasOne<Institution>()
               .WithMany()
               .HasForeignKey(a => a.InstitutionId)
               .OnDelete(DeleteBehavior.Restrict);

        // Plain columns
        builder.Property(a => a.AccountNumber)
               .HasMaxLength(34)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(a => a.Currency)
               .HasColumnType("char(3)")
               .IsUnicode(false)
               .IsRequired();

        builder.Property(a => a.Status)
               .HasConversion<string>()
               .HasMaxLength(10)
               .IsRequired();

        builder.Property(a => a.OpenedAt)
               .IsRequired();

        builder.HasIndex(a => new
               {
                   a.InstitutionId,
                   a.AccountNumber
               })
               .IsUnique();

        builder.HasIndex(a => new
               {
                   a.InstitutionId,
                   a.Currency
               })
               .IsUnique();
    }
}
