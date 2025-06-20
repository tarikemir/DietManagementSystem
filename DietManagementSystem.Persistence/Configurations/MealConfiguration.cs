using DietManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DietManagementSystem.Persistence.Configurations;

public class MealConfiguration : IEntityTypeConfiguration<Meal>
{
    public void Configure(EntityTypeBuilder<Meal> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(m => m.StartTime)
            .IsRequired();
        builder.Property(m => m.EndTime)
            .IsRequired();
        builder.Property(m => m.Contents)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasOne(m => m.DietPlan)
            .WithMany(dp => dp.Meals)
            .HasForeignKey(m => m.DietPlanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
