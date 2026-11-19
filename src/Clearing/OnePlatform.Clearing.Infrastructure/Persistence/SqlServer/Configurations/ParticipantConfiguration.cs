
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnePlatform.Clearing.Domain.Entities;

namespace OnePlatform.Clearing.Infrastructure.Persistence.SqlServer.Configurations;

public sealed class ParticipantConfiguration
    : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.ToTable("Participants");

        builder.HasKey(p => p.Id);

        // Payment relationship is configured from PaymentConfiguration.
        builder.Property(p => p.PaymentId)
               .IsRequired();

        builder.HasOne<Institution>()
               .WithMany()
               .HasForeignKey(p => p.InstitutionId)
               .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<Account>()
               .WithMany()
               .HasForeignKey(p => new { p.AccountId, p.InstitutionId })
               .HasPrincipalKey(a => new { a.Id, a.InstitutionId })
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasAlternateKey(a => new { a.Id, a.InstitutionId });

        builder.Property(p => p.Role)
               .HasConversion<string>()
               .HasMaxLength(50)
               .IsRequired();

        builder.HasIndex(p => new
               {
                   p.PaymentId,
                   p.InstitutionId
               })
               .IsUnique();
    }
}
