using DietManagementSystem.Application.Common;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DietManagementSystem.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private readonly DietManagementSystemDbContext _context;

    public IRepository<Client> Clients { get; }
    public IRepository<Dietitian> Dietitians { get; }
    public IRepository<DietPlan> DietPlans { get; }
    public IRepository<Meal> Meals { get; }

    public UnitOfWork(
        DietManagementSystemDbContext context,
        IRepository<Client> clients,
        IRepository<Dietitian> dietitians,
        IRepository<DietPlan> dietPlans,
        IRepository<Meal> meals)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        Clients = clients ?? throw new ArgumentNullException(nameof(clients));
        Dietitians = dietitians ?? throw new ArgumentNullException(nameof(dietitians));
        DietPlans = dietPlans ?? throw new ArgumentNullException(nameof(dietPlans));
        Meals = meals ?? throw new ArgumentNullException(nameof(meals));
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task ExecuteInTransactionAsync(Func<Task> operation)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await operation();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}

