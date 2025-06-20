namespace DietManagementSystem.Domain.Entities.Common;

public abstract class BaseEntity<TKey> : IAuditable where TKey : struct
{
    public TKey Id { get; set; }

    public string CreatedBy { get; set; } = null!;
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
