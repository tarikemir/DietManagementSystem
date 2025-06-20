using DietManagementSystem.Application.Common;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence;

namespace DietManagementSystem.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DietManagementSystemDbContext _context;
    public IRepository<Client> Clients { get; }
    public IRepository<Dietitian> Dietitians { get; }
    public IRepository<DietPlan> DietPlans { get; }
    public IRepository<Meal> Meals { get; }

    public UnitOfWork(DietManagementSystemDbContext context)
    {
        _context = context;
        Clients = new Repository<Client>(_context);
        Dietitians = new Repository<Dietitian>(_context);
        DietPlans = new Repository<DietPlan>(_context);
        Meals = new Repository<Meal>(_context);
    }
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
    public void Dispose()
    {
        _context.Dispose();
    }
}
