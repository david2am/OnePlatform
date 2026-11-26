using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnePlatform.Clearing.Payments.Domain.Entities;
using OnePlatform.Clearing.Payments.Domain.ValueObjects;

namespace OnePlatform.Clearing.Payments.Infrastructure.Persistence.SqlServer.Configurations;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
               .ValueGeneratedNever();

        // ISO 20022 identifiers

        // Sender: the institution that sent the message that brought this payment in.
        // A MessageId is only unique per sender, so we need it for the uniqueness rule below.
        // (InstitutionId already has a converter in ClearingDbContext.ConfigureConventions)
        builder.Property(p => p.SenderInstitutionId)
            .IsRequired();

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

        // ------------------------------------------------------------------
        // Uniqueness rules
        // ------------------------------------------------------------------

        // Rule 1. Inside one message, a sender cannot reuse the same EndToEndId.
        //  - The sender is part of the key because a MessageId is only unique per sender.
        //  - NOTPROVIDED is excluded: many payments can legitimately carry it.
        //    (Duplicates with NOTPROVIDED must be detected some other way.)
        //  - Domain code should check for the duplicate BEFORE inserting;
        //    this index is the last safety net for two requests arriving at the same time.
        builder.HasIndex(p => new { p.SenderInstitutionId, p.MessageId, p.EndToEndId })
            .IsUnique()
            .HasFilter("[EndToEndId] <> 'NOTPROVIDED'")
            .HasDatabaseName("UX_Payments_Sender_Message_EndToEnd");

        // Lookup only (NOT unique): find payments by their EndToEndId,
        // for example when a return or a status report refers to the original payment.
        builder.HasIndex(p => p.EndToEndId)
            .HasDatabaseName("IX_Payments_EndToEndId");

        // Rule 2 (OPTIONAL, business decision): an original payment can be returned only once.
        // Enable it only if partial returns are NOT allowed in OnePlatform.
        // The filter is needed because SQL Server treats NULLs as equal in a unique index.
        //
        // builder.HasIndex(p => p.ReturnOf)
        //     .IsUnique()
        //     .HasFilter("[ReturnOf] IS NOT NULL")
        //     .HasDatabaseName("UX_Payments_ReturnOf");

        // protects against two processes changing the same payment at once.
        builder.Property<byte[]>("RowVersion").IsRowVersion();
    }
}