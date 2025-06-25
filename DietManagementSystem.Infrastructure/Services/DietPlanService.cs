using DietManagementSystem.Application.Common;
using DietManagementSystem.Application.Features.DietPlan.CreateDietPlan;
using DietManagementSystem.Application.Features.DietPlan.GetDietPlan;
using DietManagementSystem.Application.Features.DietPlan.GetDietPlansByClient;
using DietManagementSystem.Application.Features.DietPlan.UpdateDietPlan;
using DietManagementSystem.Application.Features.Meal.CreateMeal;
using DietManagementSystem.Application.Services;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DietManagementSystem.Infrastructure.Services;

public class DietPlanService : IDietPlanService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILoggingService _loggingService;

    public DietPlanService(
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager,
        ILoggingService loggingService)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _loggingService = loggingService;
    }

    public async Task<Result<CreateDietPlanCommandResponse>> CreateDietPlanAsync(
        CreateDietPlanCommand request)
    {
        try
        {
            var dietPlan = new DietPlan
            {
                Title = request.Title,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                InitialWeight = request.InitialWeight,
                TargetWeight = request.TargetWeight,
                ClientId = request.ClientId,
                DietitianId = request?.DietitianId ?? Guid.Empty,
                CreatedBy = "System"
            };

            await _unitOfWork.DietPlans.AddAsync(dietPlan);
            await _unitOfWork.SaveChangesAsync();

            return Result<CreateDietPlanCommandResponse>.Success(await MapToDietPlanDto<CreateDietPlanCommandResponse>(dietPlan));

        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error creating diet plan for client ID: {ClientId}", request.ClientId);
            return Result<CreateDietPlanCommandResponse>.Failure($"Failed to create diet plan: {ex.Message}");
        }
    }

    public async Task<Result<UpdateDietPlanCommandResponse>> UpdateDietPlanAsync(UpdateDietPlanCommand request)
    {
        try
        {
            var dietPlan = await _unitOfWork.DietPlans.Query()
                .Include(dp => dp.Client)
                .Include(dp => dp.Dietitian)
                .FirstOrDefaultAsync(dp => dp.Id == request.Id);

            if (dietPlan == null)
                return Result<UpdateDietPlanCommandResponse>.Failure("Diet plan not found");

            dietPlan.Title = request.Title;
            dietPlan.StartDate = request.StartDate;
            dietPlan.EndDate = request.EndDate;
            dietPlan.InitialWeight = request.InitialWeight;
            dietPlan.TargetWeight = request.TargetWeight;
            dietPlan.DietitianId = request.DietitianId != Guid.Empty ? request.DietitianId : Guid.Empty;
            dietPlan.ClientId = request.ClientId;

            await _unitOfWork.SaveChangesAsync();

            return Result<UpdateDietPlanCommandResponse>.Success(await MapToDietPlanDto<UpdateDietPlanCommandResponse>(dietPlan));

        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error updating diet plan with ID: {DietPlanId}", request.Id);
            return Result<UpdateDietPlanCommandResponse>.Failure($"Failed to update diet plan: {ex.Message}");
        }
    }

    public async Task<Result> DeleteDietPlanAsync(Guid id)
    {
        try 
        {
            var dietPlan = await _unitOfWork.DietPlans.FirstOrDefaultAsync(dp => dp.Id == id);

            if (dietPlan == null)
                return Result.Failure("Diet plan not found");

            await _unitOfWork.DietPlans.DeleteAsync(dietPlan.Id);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error deleting diet plan with ID: {DietPlanId}", id);
            return Result.Failure($"Failed to delete diet plan: {ex.Message}");
        }
        
    }

    public async Task<Result<GetDietPlanQueryResponse>> GetDietPlanAsync(Guid id)
    {
        try
        {
            var dietPlan = await _unitOfWork.DietPlans.Query()
            .Include(dp => dp.Client)
            .Include(dp => dp.Dietitian)
                .Include(dp => dp.Meals)
            .FirstOrDefaultAsync(dp => dp.Id == id);

            
            if (dietPlan == null)
                return Result<GetDietPlanQueryResponse>.Failure("Diet plan not found");

            return Result<GetDietPlanQueryResponse>.Success(await MapToDietPlanDto<GetDietPlanQueryResponse>(dietPlan));
        }
        catch(Exception ex)
        {
            _loggingService.LogError(ex, "Error retrieving diet plan with ID: {DietPlanId}", id);
            return Result<GetDietPlanQueryResponse>.Failure($"Failed to retrieve diet plan: {ex.Message}");
        }
    }

    public async Task<Result<List<GetDietPlansByClientQueryResponse>>> GetDietPlansByClientAsync(Guid clientId)
    {
        try
        {
            var dietPlans = await _unitOfWork.DietPlans.Query()
                .Where(dp => dp.ClientId == clientId)
                .Include(dp => dp.Client)
                .Include(dp => dp.Dietitian)
                .ToListAsync();

            var response = dietPlans.Select(dp => new GetDietPlansByClientQueryResponse
            {
                Id = dp.Id,
                Title = dp.Title,
                StartDate = dp.StartDate,
                EndDate = dp.EndDate,
                InitialWeight = dp.InitialWeight,
                TargetWeight = dp.TargetWeight,
                ClientId = dp.ClientId,
                ClientName = $"{dp.Client?.FirstName} {dp.Client?.LastName}",
                DietitianId = dp.DietitianId,
                DietitianName = $"{dp.Dietitian?.FirstName} {dp.Dietitian?.LastName}"
            }).ToList();

            return Result<List<GetDietPlansByClientQueryResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error retrieving diet plans for client ID: {ClientId}", clientId);
            return Result<List<GetDietPlansByClientQueryResponse>>.Failure($"Failed to retrieve diet plans: {ex.Message}");
        }
    }

    public async Task<Result<CreateMealCommandResponse>> AddMealToDietPlanAsync(CreateMealCommand request)
    {
        try
        {
            var dietPlan = await _unitOfWork.DietPlans.FirstOrDefaultAsync(dp => dp.Id == request.DietPlanId);

            var meal = new Meal
            {
                Title = request.Title,
                ScheduledDate = request.ScheduledDate,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Contents = request.Contents,
                DietPlanId = request.DietPlanId,
                CreatedBy = "System"
            };

            await _unitOfWork.Meals.AddAsync(meal);
            await _unitOfWork.SaveChangesAsync();

            return Result<CreateMealCommandResponse>.Success(new CreateMealCommandResponse
            {
                Id = meal.Id,
                Title = meal.Title,
                ScheduledDate = meal.ScheduledDate,
                StartTime = meal.StartTime,
                EndTime = meal.EndTime,
                Contents = meal.Contents
            });
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error adding meal to diet plan with ID: {DietPlanId}", request.DietPlanId);
            return Result<CreateMealCommandResponse>.Failure($"Failed to add meal: {ex.Message}");
        }
    }

    private async Task<T> MapToDietPlanDto<T>(DietPlan dietPlan) where T : IDietPlan, new()
    {
        try
        {
            var client = await _unitOfWork.Clients.Query()
            .Include(c => c.ApplicationUser)
            .FirstOrDefaultAsync(c => c.Id == dietPlan.ClientId);

            var dietitian = await _unitOfWork.Dietitians.Query()
                .Include(d => d.ApplicationUser)
                .FirstOrDefaultAsync(d => d.Id == dietPlan.DietitianId);

            return new()
            {
                Id = dietPlan.Id,
                Title = dietPlan.Title,
                StartDate = dietPlan.StartDate,
                EndDate = dietPlan.EndDate,
                InitialWeight = dietPlan.InitialWeight,
                TargetWeight = dietPlan.TargetWeight,
                ClientId = dietPlan.ClientId,
                ClientName = $"{client?.FirstName} {client?.LastName}",
                DietitianId = dietPlan.DietitianId,
                DietitianName = $"{dietitian?.FirstName} {dietitian?.LastName}",
                Meals = dietPlan.Meals.Select(m => new CreateMealCommandResponse
                {
                    Id = m.Id,
                    Title = m.Title,
                    ScheduledDate = m.ScheduledDate,
                    StartTime = m.StartTime,
                    EndTime = m.EndTime,
                    Contents = m.Contents
                }).ToList()
            };
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error mapping diet plan to DTO for ID: {DietPlanId}", dietPlan.Id);
            throw;
        }
    }
}
