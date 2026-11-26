
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnePlatform.Clearing.Institutions.Domain.Entities;
using OnePlatform.Clearing.Institutions.Domain.ValueObjects;

namespace OnePlatform.Clearing.Institutions.Infrastructure.Persistence.SqlServer;

public sealed class InstitutionConfiguration
    : IEntityTypeConfiguration<Institution>
{
    public void Configure(EntityTypeBuilder<Institution> builder)
    {
        builder.ToTable("Institutions", table =>
        {
            table.HasCheckConstraint(
                "CK_Institutions_Membership",
                "[IsMember] = 0 OR [MembershipSince] IS NOT NULL");
        });

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(i => i.RoutingNumber)
               .HasConversion(
                   routingNumber => routingNumber.Value,
                   value => RoutingNumber.Create(value).Value)
               .HasColumnType("char(9)")
               .IsUnicode(false)
               .IsRequired();

        builder.HasIndex(i => i.RoutingNumber) // unique per Clearing System
               .IsUnique();

        builder.Property(i => i.IsMember)
               .IsRequired();

        builder.Property(i => i.MembershipSince)
               .IsRequired(false);
    }
}