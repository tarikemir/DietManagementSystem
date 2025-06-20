using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Domain.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DietManagementSystem.Persistence;

public class DietManagementSystemDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public DbSet<Dietitian> Dietitians => Set<Dietitian>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<DietPlan> DietPlans => Set<DietPlan>();
    public DbSet<Meal> Meals => Set<Meal>();

    public DietManagementSystemDbContext(DbContextOptions<DietManagementSystemDbContext> options) : base(options) { }

    public DietManagementSystemDbContext(
        DbContextOptions<DietManagementSystemDbContext> options, 
        IHttpContextAccessor httpContextAccessor) 
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}
