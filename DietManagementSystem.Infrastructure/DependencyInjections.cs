using DietManagementSystem.Application.Common;
using DietManagementSystem.Application.Services;
using DietManagementSystem.Infrastructure.Repositories;
using DietManagementSystem.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DietManagementSystem.Infrastructure;

public static class DependencyInjections
{
    public static IServiceCollection AddInfrastructure( this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDietPlanService, DietPlanService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<ILoggingService, LoggingService>();

        return services;
    }
}
