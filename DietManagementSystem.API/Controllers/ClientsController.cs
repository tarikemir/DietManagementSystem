using DietManagementSystem.Application.Features.Client.CreateClient;
using DietManagementSystem.Application.Features.Client.DeleteClient;
using DietManagementSystem.Application.Features.Client.GetClient;
using DietManagementSystem.Application.Features.Client.GetClients;
using DietManagementSystem.Application.Features.Client.GetClientsByDietitian;
using DietManagementSystem.Application.Features.Client.UpdateClient;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize(Roles = "Dietitian,Admin")]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientCommand request)
        {
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [HttpPut]
        [Authorize(Roles = "Dietitian,Admin")]
        public async Task<IActionResult> UpdateClient([FromBody] UpdateClientCommand request)
        {

            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Dietitian,Admin")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            var result = await _mediator.Send(new DeleteClientCommand { ClientId = id });
            return HandleResult(result);
        }

        [HttpGet]
        [Authorize(Roles = "Dietitian,Admin")]
        public async Task<IActionResult> GetClients()
        {
            var result = await _mediator.Send(new GetClientsQuery { });
            return HandleResult(result);

        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Dietitian,Admin")]
        public async Task<IActionResult> GetClient(Guid id)
        {
            var result = await _mediator.Send(new GetClientByIdQuery { ClientId = (Guid) id });
            return HandleResult(result);
        }

        [HttpGet("dietitian/{dietitianId?}")]
        [Authorize(Roles = "Dietitian,Admin")]
        public async Task<IActionResult> GetClientsByDietitian(Guid dietitianId)
        {
            var result = await _mediator.Send(new GetClientsByDietitianQuery { DietitianId = dietitianId });
            return HandleResult(result);
        }
        private IActionResult HandleResult<T>(Result<T> result)
        {
            return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
        }
    }
}