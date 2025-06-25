namespace DietManagementSystem.Application.Features.Dietitian.GetDietitianById;

public class GetDietitianByIdQueryResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;
    public Guid ApplicationUserId { get; set; }
    public int ClientCount { get; set; }
    public int DietPlanCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
