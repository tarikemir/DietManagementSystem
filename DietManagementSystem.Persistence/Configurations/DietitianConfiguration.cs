using DietManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DietManagementSystem.Persistence.Configurations;

public class DietitianConfiguration : IEntityTypeConfiguration<Dietitian>
{
    public void Configure(EntityTypeBuilder<Dietitian> builder)
    {
        builder.HasKey( d => d.Id);
        builder.Property( d => d.FirstName)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(d => d.LastName)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(d => d.Email)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(d => d.PhoneNumber)
            .IsRequired()
            .HasMaxLength(15);

        builder.HasMany( d => d.Clients)
            .WithOne(c => c.Dietitian)
            .HasForeignKey(c => c.DietitianId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasMany(d => d.DietPlans)
            .WithOne(dp => dp.Dietitian)
            .HasForeignKey(dp => dp.DietitianId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasOne(d => d.ApplicationUser)
            .WithOne(u => u.Dietitian)
            .HasForeignKey<Dietitian>(d => d.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
