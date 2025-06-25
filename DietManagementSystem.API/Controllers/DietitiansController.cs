using DietManagementSystem.Application.Features.Dietitian.CreateDietitian;
using DietManagementSystem.Application.Features.Dietitian.DeleteDietitian;
using DietManagementSystem.Application.Features.Dietitian.GetDietitianById;
using DietManagementSystem.Application.Features.Dietitian.GetDietitians;
using DietManagementSystem.Application.Features.Dietitian.UpdateDietitian;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DietManagementSystem.API.Controllers;

[Authorize]
[Route("api/v1/dietitians")]
[ApiController]
public class DietitiansController : ControllerBase
{
    private readonly IMediator _mediator;

    public DietitiansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateDietitian([FromBody] CreateDietitianCommand command)
    {
        var result = await _mediator.Send(command);
        return HandleResult(result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetDietitians()
    {
        var result = await _mediator.Send(new GetDietitiansQuery());
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetDietitian(Guid id)
    {
            var result = await _mediator.Send(new GetDietitianByIdQuery { Id = (Guid) id });
            return HandleResult(result);
    }
    

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateDietitian([FromBody] UpdateDietitianCommand command)
    {
        var result = await _mediator.Send(command);
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteDietitian(Guid id)
    {
        var result = await _mediator.Send(new DeleteDietitianCommand { Id = id });
        return HandleResult(result);
    }

    private IActionResult HandleResult<T>(Result<T> result)
    {
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }
}

