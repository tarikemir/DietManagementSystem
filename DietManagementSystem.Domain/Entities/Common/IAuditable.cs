namespace DietManagementSystem.Domain.Entities.Common;

public interface IAuditable
{
    string CreatedBy { get; set; }
    DateTime CreatedAt { get; set; }

    string? UpdatedBy { get; set; }
    DateTime? UpdatedAt { get; set; }
}
