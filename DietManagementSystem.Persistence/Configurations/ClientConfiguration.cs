using DietManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DietManagementSystem.Persistence.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey( e => e.Id);
        builder.Property( e => e.FirstName).IsRequired()
            .HasMaxLength(50);
        builder.Property(e => e.LastName).IsRequired()
            .HasMaxLength(50);
        builder.Property(e => e.Email).IsRequired()
            .HasMaxLength(100);
        builder.Property(e => e.PhoneNumber).HasMaxLength(15);
        builder.Property(e => e.InitialWeight).IsRequired();

        builder.HasOne( e => e.Dietitian)
            .WithMany(d => d.Clients)
            .HasForeignKey(e => e.DietitianId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasMany( e => e.DietPlans)
            .WithOne(dp => dp.Client)
            .HasForeignKey(dp => dp.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.ApplicationUser)
            .WithOne(u => u.Client)
            .HasForeignKey<Client>(c => c.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
