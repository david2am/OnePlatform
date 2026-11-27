using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnePlatform.Clearing.Accounts.Domain.Entities;
using OnePlatform.Clearing.Institutions.Domain.Entities;

namespace OnePlatform.Clearing.Accounts.Infrastructure.Persistence.SqlServer.Configurations;

public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
               .ValueGeneratedNever();

        // Foreign key to Institution
        builder.HasOne<Institution>()
               .WithMany()
               .HasForeignKey(a => a.InstitutionId)
               .OnDelete(DeleteBehavior.Restrict);

        // Plain columns
        builder.Property(a => a.AccountNumber)
               .HasMaxLength(17)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(a => a.Currency)
               .IsRequired();

        builder.Property(a => a.Status)
               .HasConversion<string>()
               .HasMaxLength(10)
               .IsRequired();

        builder.Property(a => a.OpenedAt)
               .IsRequired();

        // Rule 1: an account number is unique inside one institution
        builder.HasIndex(a => new { a.InstitutionId, a.AccountNumber })
               .IsUnique();

        // Rule 2: one account per currency per institution
        builder.HasIndex(a => new { a.InstitutionId, a.Currency })
               .IsUnique();

        // should NOT block opening a new one in the same currency:
        //
        // builder.HasIndex(a => new { a.InstitutionId, a.Currency })
        //        .IsUnique()
        //        .HasFilter("[Status] <> 'Closed'");
    }
}