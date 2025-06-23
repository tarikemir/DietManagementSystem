using DietManagementSystem.Domain.Entities;

namespace DietManagementSystem.Application.Common;

public interface IUnitOfWork
{
    IRepository<Client> Clients { get; }
    IRepository<Dietitian> Dietitians { get; }
    IRepository<DietPlan> DietPlans { get; }
    IRepository<Meal> Meals { get; }


    Task<int> SaveChangesAsync();
}
