using DietManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DietManagementSystem.Persistence.Configurations;

public class DietPlanConfiguration : IEntityTypeConfiguration<DietPlan>
{
    public void Configure(EntityTypeBuilder<DietPlan> builder)
    {
        builder.HasKey(dp => dp.Id);
        builder.Property(dp => dp.Title)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(dp => dp.StartDate)
            .IsRequired();
        builder.Property(dp => dp.EndDate)
            .IsRequired();
        builder.Property(dp => dp.InitialWeight)
            .IsRequired();

        builder.HasOne(dp => dp.Client)
            .WithMany(c => c.DietPlans)
            .HasForeignKey(dp => dp.ClientId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(dp => dp.Dietitian)
            .WithMany(d => d.DietPlans)
            .HasForeignKey(dp => dp.DietitianId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasMany(dp => dp.Meals)
            .WithOne(m => m.DietPlan)
            .HasForeignKey(m => m.DietPlanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
