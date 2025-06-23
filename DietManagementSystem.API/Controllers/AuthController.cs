using DietManagementSystem.Application.Features.Auth.Login;
using DietManagementSystem.Application.Features.Auth.RegisterClient;
using DietManagementSystem.Application.Features.Auth.RegisterDietitian;
using DietManagementSystem.Application.Services;
using DietManagementSystem.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DietManagementSystem.Api.Controllers;

[Route("api/v1/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMediator _mediator;
    public AuthController(IAuthService authService, IMediator mediator)
    {
        _authService = authService;
        _mediator = mediator;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand request, IMediator _mediator)
    {
        var result = await _mediator.Send(request);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpPost("register/client")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterClient([FromBody] RegisterClientCommand request, IMediator _mediator)
    {
        var result = await _mediator.Send(request);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpPost("register/dietitian")]
    [Authorize(Roles = nameof(UserType.Admin))]
    public async Task<IActionResult> RegisterDietitian([FromBody] RegisterDietitianCommand request, IMediator _mediator)
    {
        var result = await _mediator.Send(request);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Value);
    }
}