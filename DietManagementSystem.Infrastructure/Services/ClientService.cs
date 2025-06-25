using DietManagementSystem.Application.Common;
using DietManagementSystem.Application.Features.Client.CreateClient;
using DietManagementSystem.Application.Features.Client.GetClient;
using DietManagementSystem.Application.Features.Client.GetClientsByDietitian;
using DietManagementSystem.Application.Features.Client.UpdateClient;
using DietManagementSystem.Application.Features.DietPlan.GetDietPlan;
using DietManagementSystem.Application.Features.Meal.CreateMeal;
using DietManagementSystem.Application.Services;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DietManagementSystem.Infrastructure.Services;

public class ClientService : IClientService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILoggingService _loggingService;

    public ClientService(
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager,
        ILoggingService loggingService)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _loggingService = loggingService;
    }

    public async Task<Result<CreateClientCommandResponse>> CreateClientAsync(CreateClientCommand request)
    {
        try
        {

            var client = new Client
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                InitialWeight = request.InitialWeight,
                DietitianId = request.DietitianId
            };

            await _unitOfWork.Clients.AddAsync(client);
            await _unitOfWork.SaveChangesAsync();

            return Result<CreateClientCommandResponse>.Success(new CreateClientCommandResponse
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                InitialWeight = client.InitialWeight,
                DietitianId = client.DietitianId
            });
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error creating client for dietitian ID: {DietitianId}", request.DietitianId);
            return Result<CreateClientCommandResponse>.Failure($"Failed to create client: {ex.Message}");
        }
    }

    public async Task<Result<UpdateClientCommandResponse>> UpdateClientAsync(UpdateClientCommand request)
    {
        try
        {
            var client = await _unitOfWork.Clients.FirstOrDefaultAsync(c => c.Id == request.ClientId);
            if (client == null)
            {
                _loggingService.LogWarning("Client not found with ID: {ClientId}", request.ClientId);
                return Result<UpdateClientCommandResponse>.Failure("Client not found");
            }

            client.FirstName = request.FirstName;
            client.LastName = request.LastName;
            client.InitialWeight = request.InitialWeight;

            await _unitOfWork.SaveChangesAsync();

            return Result<UpdateClientCommandResponse>.Success(new UpdateClientCommandResponse
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                InitialWeight = client.InitialWeight,
                DietitianId = client.DietitianId
            });
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error updating client with ID: {ClientId}", request.ClientId);
            return Result<UpdateClientCommandResponse>.Failure($"Failed to update client: {ex.Message}");
        }
    }

    public async Task<Result> DeleteClientAsync(Guid id)
    {
        try
        {
            var client = await _unitOfWork.Clients.FirstOrDefaultAsync(c => c.Id == id);
            if (client == null)
            {
                _loggingService.LogWarning("Client not found for deletion with ID: {ClientId}", id);
                return Result.Failure("Client not found");
            }

            await _unitOfWork.Clients.DeleteAsync(client.Id);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error deleting client with ID: {ClientId}", id);
            return Result.Failure($"Failed to delete client: {ex.Message}");
        }
    }

    public async Task<Result<GetClientByIdQueryResponse>> GetClientAsync(Guid id)
    {
        try
        {
            var client = await _unitOfWork.Clients.Query()
                .Include(c => c.ApplicationUser)
                .Include(c => c.Dietitian)
                .Include(c => c.DietPlans)
                    .ThenInclude(dp => dp.Meals)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
            {
                _loggingService.LogWarning("Client not found with ID: {ClientId}", id);
                return Result<GetClientByIdQueryResponse>.Failure("Client not found");
            }

            _loggingService.LogDebug("Successfully retrieved client with ID: {ClientId}, Dietitian: {DietitianName}",
                client.Id, $"{client.Dietitian?.FirstName} {client.Dietitian?.LastName}");

            return Result<GetClientByIdQueryResponse>.Success(new GetClientByIdQueryResponse
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.ApplicationUser.Email,
                InitialWeight = client.InitialWeight,
                DietitianId = client.DietitianId,
                DietitianName = $"{client.Dietitian?.FirstName} {client.Dietitian?.LastName}",
                DietPlans = client.DietPlans.Select(dp => new GetDietPlanQueryResponse
                {
                    Id = dp.Id,
                    Title = dp.Title,
                    StartDate = dp.StartDate,
                    EndDate = dp.EndDate,
                    InitialWeight = dp.InitialWeight,
                    TargetWeight = dp.TargetWeight,
                    ClientId = dp.ClientId,
                    ClientName = client.FirstName + " " + client.LastName,
                    DietitianId = client.DietitianId,
                    DietitianName = client.Dietitian.FirstName + " " + client.Dietitian.LastName,
                    Meals = dp.Meals.Select(m => new CreateMealCommandResponse
                    {
                        Id = m.Id,
                        Contents = m.Contents,
                        Title = m.Title,
                        ScheduledDate = m.ScheduledDate,
                        StartTime = m.StartTime,
                        EndTime = m.EndTime,
                    }).ToList()
                }).ToList()
            });
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error retrieving client with ID: {ClientId}", id);
            return Result<GetClientByIdQueryResponse>.Failure($"Failed to retrieve client: {ex.Message}");
        }
    }

    public async Task<Result<List<GetClientsByDietitianQueryResponse>>> GetClientsByDietitianAsync(Guid dietitianId)
    {
        try
        {
            var clients = await _unitOfWork.Clients.Query()
                .Where(c => c.DietitianId == dietitianId)
                .Include(c => c.Dietitian)
                .Include(c => c.ApplicationUser)
                .ToListAsync();

            var response = clients.Select(c => new GetClientsByDietitianQueryResponse
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.ApplicationUser.Email,
                InitialWeight = c.InitialWeight,
                DietitianId = c.DietitianId,
                DietitianName = $"{c.Dietitian?.FirstName} {c.Dietitian?.LastName}",
                ApplicationUserId = c.ApplicationUser.Id
            }).ToList();

            return Result<List<GetClientsByDietitianQueryResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error retrieving clients for dietitian ID: {DietitianId}", dietitianId);
            return Result<List<GetClientsByDietitianQueryResponse>>.Failure($"Failed to retrieve clients: {ex.Message}");
        }
    }
}
