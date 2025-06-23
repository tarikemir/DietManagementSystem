using DietManagementSystem.Application.Features.Client.CreateClient;
using DietManagementSystem.Application.Features.Client.DeleteClient;
using DietManagementSystem.Application.Features.Client.GetClient;
using DietManagementSystem.Application.Features.Client.GetClientsByDietitian;
using DietManagementSystem.Application.Features.Client.UpdateClient;
using DietManagementSystem.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DietManagementSystem.Controllers
{
    [Authorize]
    [Route("api/v1/clients")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthorizationService _authorizationService;

        public ClientController(
            IMediator mediator,
            IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Authorize(Roles = "DietitianOrAdmin")]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientCommand request)
        {
            if (User.IsInRole(UserType.Dietitian.ToString()))
            {
                request.DietitianId = GetCurrentUserId();
            }

            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [HttpPut]
        [Authorize(Roles = "DietitianOrAdmin")]
        public async Task<IActionResult> UpdateClient([FromBody] UpdateClientCommand request)
        {
            if (User.IsInRole(UserType.Dietitian.ToString()))
            {
                var client = await _mediator.Send(new GetClientByIdQuery { ClientId = request.ClientId });
                if (client.IsFailure || client.Value.DietitianId != GetCurrentUserId())
                    return Forbid();
            }

            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "DietitianOrAdmin")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            if (User.IsInRole(UserType.Dietitian.ToString()))
            {
                var client = await _mediator.Send(new GetClientByIdQuery { ClientId = id });
                if (client.IsFailure || client.Value.DietitianId != GetCurrentUserId())
                    return Forbid();
            }

            var result = await _mediator.Send(new DeleteClientCommand { ClientId = id });
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient(Guid id)
        {
            var result = await _mediator.Send(new GetClientByIdQuery { ClientId = id });

            if (result.IsSuccess && User.IsInRole(UserType.Dietitian.ToString()))
            {
                if (result.Value.DietitianId != GetCurrentUserId())
                    return Forbid();
            }
            return HandleResult(result);
        }

        [HttpGet("dietitian/{dietitianId?}")]
        public async Task<IActionResult> GetClientsByDietitian(Guid? dietitianId = null)
        {
            if (User.IsInRole(UserType.Dietitian.ToString()))
            {
                var currentDietitianId = GetCurrentUserId();
                if (dietitianId.HasValue && dietitianId.Value != currentDietitianId)
                    return Forbid();

                dietitianId = currentDietitianId;
            }
            else if (!dietitianId.HasValue && User.IsInRole(UserType.Admin.ToString()))
            {
                dietitianId = null;
            }

            var query = dietitianId.HasValue
                ? new GetClientsByDietitianQuery { DietitianId = dietitianId.Value }
                : null;

            var result = query != null
                ? await _mediator.Send(query)
                : Result<List<GetClientsByDietitianQueryResponse>>.Failure("Dietitian ID required for non-admin users");

            return HandleResult(result);
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(userIdClaim);
        }

        private IActionResult HandleResult<T>(Result<T> result)
        {
            return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
        }
    }
}