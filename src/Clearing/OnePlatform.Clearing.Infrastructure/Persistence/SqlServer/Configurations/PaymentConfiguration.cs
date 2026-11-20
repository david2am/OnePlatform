using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnePlatform.Clearing.Domain.Entities;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Infrastructure.Persistence.SqlServer.Configurations;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
               .ValueGeneratedNever();

        // ISO 20022 identifiers
        builder.Property(p => p.MessageId)
            .HasConversion(
                id => id.Value,
                value => MessageId.Create(value).Value)
            .HasMaxLength(35)
            .IsRequired();

        builder.Property(p => p.EndToEndId)
            .HasConversion(
                id => id.Value,
                value => EndToEndId.Create(value).Value)
            .HasMaxLength(35)
            .IsRequired();

        // Return relationship (a return points to the original payment)
        builder.HasOne<Payment>()
            .WithMany()
            .HasForeignKey(p => p.ReturnOf)
            .OnDelete(DeleteBehavior.Restrict);

        // Payment properties
        builder.Property(p => p.PaymentType)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.InitiatedAt)
            .IsRequired();

        // Debtor
        builder.OwnsOne(p => p.Debtor, debtor =>
        {
            debtor.Property(d => d.Name)
                .HasColumnName("DebtorName")
                .HasMaxLength(200)
                .IsRequired();

            debtor.Property(d => d.AccountNumber)
                .HasColumnName("DebtorAccountNumber")
                .HasMaxLength(50)
                .IsRequired();

            debtor.Property(d => d.InstitutionId)
                .HasColumnName("DebtorInstitutionId")
                .IsRequired();
        });

        builder.Navigation(p => p.Debtor)
            .IsRequired();

        // Creditor
        builder.OwnsOne(p => p.Creditor, creditor =>
        {
            creditor.Property(c => c.Name)
                .HasColumnName("CreditorName")
                .HasMaxLength(200)
                .IsRequired();

            creditor.Property(c => c.AccountNumber)
                .HasColumnName("CreditorAccountNumber")
                .HasMaxLength(50)
                .IsRequired();

            creditor.Property(c => c.InstitutionId)
                .HasColumnName("CreditorInstitutionId")
                .IsRequired();
        });

        builder.Navigation(p => p.Creditor)
            .IsRequired();

        // Money
        builder.OwnsOne(p => p.Amount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("Amount")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("Currency")
                .IsRequired();
        });

        builder.Navigation(p => p.Amount)
            .IsRequired();

        // Participants
        builder.HasMany(p => p.Participants)
            .WithOne()
            .HasForeignKey(p => p.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(p => p.Participants)
            .HasField("_participants")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Validation results
        builder.HasMany(p => p.ValidationResults)
            .WithOne()
            .HasForeignKey(v => v.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(p => p.ValidationResults)
            .HasField("_validationResults")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Status events
        builder.HasMany(p => p.StatusEvents)
            .WithOne()
            .HasForeignKey(s => s.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(p => p.StatusEvents)
            .HasField("_statusEvents")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Rule 1. A payment is identified inside a message by its EndToEndId,
        builder.HasIndex(p => new { p.MessageId, p.EndToEndId })
            .IsUnique();

        // Optional: protects against two processes changing the same payment at once.
        // Needs `public byte[] RowVersion { get; private set; } = [];` in Payment.
        // builder.Property<byte[]>("RowVersion").IsRowVersion();
    }
}