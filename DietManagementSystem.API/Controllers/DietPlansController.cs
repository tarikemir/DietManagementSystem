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
    [Authorize(Roles = "Dietitian,Admin")]
    public async Task<IActionResult> CreateDietPlan([FromBody] CreateDietPlanCommand request)
    {
        var result = await _mediator.Send(request);
        return HandleResult(result);
    }

    [HttpPut]
    [Authorize(Roles = "Dietitian,Admin")]
    public async Task<IActionResult> UpdateDietPlan([FromBody] UpdateDietPlanCommand request)
    {
        var result = await _mediator.Send(request);
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Dietitian,Admin")]
    public async Task<IActionResult> DeleteDietPlan(Guid id)
    {
        var result = await _mediator.Send(new DeleteDietPlanCommand { Id = id});
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDietPlan(Guid id)
    {
        var result = await _mediator.Send(new GetDietPlanQuery { Id = id});
        return HandleResult(result);
    }

    [HttpGet("client/{clientId}")]
    public async Task<IActionResult> GetDietPlansByClient(Guid clientId)
    {
        var result = await _mediator.Send(new GetDietPlansByClientQuery { ClientId = clientId });
        return HandleResult(result);
    }

    [HttpPost("meal")]
    [Authorize(Roles = "Dietitian,Admin")]
    public async Task<IActionResult> AddMealToDietPlan([FromBody] CreateMealCommand request)
    {
        var result = await _dietPlanService.AddMealToDietPlanAsync(request);
        return HandleResult(result);
    }

    private IActionResult HandleResult<T>(Result<T> result)
    {
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }
}