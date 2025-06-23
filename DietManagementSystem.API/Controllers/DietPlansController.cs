using DietManagementSystem.Application.Features.DietPlan.CreateDietPlan;
using DietManagementSystem.Application.Features.DietPlan.DeleteDietPlan;
using DietManagementSystem.Application.Features.DietPlan.GetDietPlan;
using DietManagementSystem.Application.Features.DietPlan.GetDietPlansByClient;
using DietManagementSystem.Application.Features.DietPlan.UpdateDietPlan;
using DietManagementSystem.Application.Features.Meal.CreateMeal;
using DietManagementSystem.Application.Services;
using DietManagementSystem.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DietManagementSystem.Controllers;

[Authorize]
[Route("api/v1/diet-plans")]
[ApiController]
public class DietPlanController : ControllerBase
{
    private readonly IDietPlanService _dietPlanService;
    private readonly IMediator _mediator;
    public DietPlanController(IDietPlanService dietPlanService, IMediator mediator)
    {
        _dietPlanService = dietPlanService;
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "DietitianOrAdmin")]
    public async Task<IActionResult> CreateDietPlan([FromBody] CreateDietPlanCommand request)
    {
        var (userId, userType) = GetCurrentUserInfo();

        if (userId.Value != request.DietitianId && userType.Value != UserType.Admin)
            return Unauthorized();

        var result = await _mediator.Send(request);
        return HandleResult(result);
    }

    [HttpPut]
    [Authorize(Roles = "DietitianOrAdmin")]
    public async Task<IActionResult> UpdateDietPlan([FromBody] UpdateDietPlanCommand request)
    {
        var (userId, userType) = GetCurrentUserInfo();

        if (userId.Value != request.DietitianId && userType.Value != UserType.Admin)
            return Unauthorized();


        var result = await _mediator.Send(request);
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "DietitianOrAdmin")]
    public async Task<IActionResult> DeleteDietPlan(DeleteDietPlanCommand request)
    {
        var (userId, userType) = GetCurrentUserInfo();
        if (userType.Value != UserType.Dietitian && userType.Value != UserType.Admin)
            return Unauthorized();

        var result = await _mediator.Send(request);
        return HandleResult(result);

    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDietPlan(GetDietPlanQuery request)
    {
        var result = await _mediator.Send(request);
        return HandleResult(result);
    }

    [HttpGet("client/{clientId}")]
    public async Task<IActionResult> GetDietPlansByClient(GetDietPlansByClientQuery request)
    {
        var (userId, userType) = GetCurrentUserInfo();
        if (userType.Value != UserType.Dietitian && userType.Value != UserType.Admin)
            return Unauthorized();

        var result = await _mediator.Send(request);
        return HandleResult(result);
    }

    [HttpPost("meal")]
    [Authorize(Roles = "DietitianOrAdmin")]
    public async Task<IActionResult> AddMealToDietPlan([FromBody] CreateMealCommand request)
    {
        var (userId, userType) = GetCurrentUserInfo();
        if (userType.Value != UserType.Dietitian && userType.Value != UserType.Admin)
            return Unauthorized();

        var result = await _dietPlanService.AddMealToDietPlanAsync(request);
        return HandleResult(result);
    }

    private (Guid?, UserType?) GetCurrentUserInfo()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userTypeClaim = User.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(userTypeClaim))
            return (null, null);

        if (!Guid.TryParse(userIdClaim, out var userId))
            return (null, null);

        if (!Enum.TryParse<UserType>(userTypeClaim, out var userType))
            return (null, null);

        return (userId, userType);
    }

    private IActionResult HandleResult<T>(Result<T> result)
    {
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }
}