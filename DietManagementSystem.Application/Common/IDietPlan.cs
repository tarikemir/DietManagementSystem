namespace DietManagementSystem.Application.Common;

public interface IDietPlan
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double InitialWeight { get; set; }
    public double TargetWeight { get; set; }
    public Guid ClientId { get; set; }
    public string ClientName { get; set; }
    public Guid DietitianId { get; set; }
    public string DietitianName { get; set; }
    public List<Domain.Entities.Meal> Meals { get; set; }
}
