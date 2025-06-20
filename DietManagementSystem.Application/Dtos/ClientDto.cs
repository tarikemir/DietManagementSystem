namespace DietManagementSystem.Application.Dtos;

public class ClientDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public double InitialWeight { get; set; }
    public Guid DietitianId { get; set; }
    public Guid ApplicationUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
