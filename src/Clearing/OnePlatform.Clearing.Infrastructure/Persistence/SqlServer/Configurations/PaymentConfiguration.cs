using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnePlatform.Clearing.Domain.Entities;

namespace OnePlatform.Clearing.Infrastructure.Persistence.SqlServer;

public sealed class PaymentConfiguration
  : IEntityTypeConfiguration<Payment>
{
  public void Configure(EntityTypeBuilder<Payment> builder)
  {
    builder.ToTable("Payments");
    builder.HasKey(p => p.Id);
    
    builder.ComplexProperty(p => p.Amount, money =>
    {
      money.Property(m => m.Amount)
           .HasColumnName("Amount")
           .HasColumnType("bigint")
           .IsRequired();

      money.Property(m => m.Currency)
           .HasColumnName("Currency")
           .HasMaxLength(3)
           .IsUnicode(false)
           .IsRequired();
    });

    builder.ComplexProperty(p => p.Debtor, party =>
    {
      party.Property("AccountNumber")
           .HasColumnName("DebtorAccountNumber")
           .IsRequired();

      party.Property("Name")
           .HasColumnName("DebtorName")
           .IsRequired();

      party.Property("InstitutionId")
           .HasColumnName("DebtorInstitutionId")
           .IsRequired();
    });

    builder.ComplexProperty(p => p.Creditor, party =>
    {
      party.Property("AccountNumber")
           .HasColumnName("CreditorAccountNumber")
           .IsRequired();

      party.Property("Name")
           .HasColumnName("CreditorName")
           .IsRequired();

      party.Property("InstitutionId")
           .HasColumnName("CreditorInstitutionId")
           .IsRequired();
    });
  }
}