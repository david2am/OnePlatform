using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnePlatform.Clearing.Domain.Entities;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Infrastructure.Persistence.SqlServer;

public sealed class PaymentConfiguration
  : IEntityTypeConfiguration<Payment>
{
  public void Configure(EntityTypeBuilder<Payment> builder)
  {
    builder.ToTable("Payments");
    builder.HasKey(p => p.Id);

    builder.Property(p => p.EndToEndId)
           .HasConversion(id => id.Value, value => EndToEndId.Create(value).Value)
           .HasMaxLength(35)
           .IsRequired();

    builder.Property(p => p.MessageId)
           .HasConversion(id => id.Value, value => MessageId.Create(value).Value)
           .HasMaxLength(35)
           .IsRequired();
    
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

    builder.HasMany(p => p.ValidationResults)
           .WithOne()
           .HasForeignKey(v => v.PaymentId);

    builder.Navigation(p => p.ValidationResults)
           .HasField("_validationResults")
           .UsePropertyAccessMode(PropertyAccessMode.Field);

    builder.HasMany(p => p.StatusEvents)
          .WithOne()
          .HasForeignKey(e => e.PaymentId);

    builder.Navigation(p => p.StatusEvents)
          .HasField("_statusEvents")
          .UsePropertyAccessMode(PropertyAccessMode.Field);
  }
}