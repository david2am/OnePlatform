using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnePlatform.Clearing.Domain.Entities;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Infrastructure.Persistence.SqlServer;

public sealed class InstitutionConfiguration
  : IEntityTypeConfiguration<Institution>
{
  public void Configure(EntityTypeBuilder<Institution> builder)
  {
    builder.ToTable("Institutions");
    builder.HasKey(i => i.Id);

    builder.Property(i => i.Id)
           .ValueGeneratedNever();

    builder.Property(i => i.Name)
           .IsRequired()
           .HasMaxLength(200);

    builder.Property(i => i.RoutingNumber)
           .HasConversion(routingNumber => routingNumber.Value, value => RoutingNumber.Create(value).Value)
           .HasMaxLength(9)
           .IsRequired();

    builder.HasIndex(i => i.RoutingNumber)
           .IsUnique();

    builder.Property(i => i.IsMember)
        .IsRequired();

    builder.Property(i => i.MembershipSince)
        .IsRequired(false);
  }
}