using DietManagementSystem.Application.Features.DietPlan.CreateDietPlan;
using DietManagementSystem.Application.Features.DietPlan.GetDietPlan;
using DietManagementSystem.Application.Features.DietPlan.GetDietPlansByClient;
using DietManagementSystem.Application.Features.DietPlan.UpdateDietPlan;
using DietManagementSystem.Application.Features.Meal.CreateMeal;
using DietManagementSystem.Domain.Enums;

namespace DietManagementSystem.Application.Services;

public interface IDietPlanService
{
    Task<Result<CreateDietPlanCommandResponse>> CreateDietPlanAsync(CreateDietPlanCommand request);
    Task<Result<UpdateDietPlanCommandResponse>> UpdateDietPlanAsync(UpdateDietPlanCommand request);
    Task<Result> DeleteDietPlanAsync(Guid id);
    Task<Result<GetDietPlanQueryResponse>> GetDietPlanAsync(Guid id);
    Task<Result<List<GetDietPlansByClientQueryResponse>>> GetDietPlansByClientAsync(Guid clientId);
    Task<Result<CreateMealCommandResponse>> AddMealToDietPlanAsync(CreateMealCommand request);
}