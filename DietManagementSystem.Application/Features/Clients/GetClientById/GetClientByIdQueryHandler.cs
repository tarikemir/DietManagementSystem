using DietManagementSystem.Application.Common;
using DietManagementSystem.Application.Dtos;
using MediatR;

namespace DietManagementSystem.Application.Features.Clients.GetClientById;

public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, Result<ClientDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetClientByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ClientDto>> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _unitOfWork.Clients.GetByIdAsync(request.Id);
            if (response is null)
                return Result<ClientDto>.Failure("Client cannot be found!");

            var clientDto = new ClientDto
            {
                Id = request.Id,
                FirstName = response.FirstName,
                LastName = response.LastName,
                Email = response.Email,
                PhoneNumber = response.PhoneNumber,
                InitialWeight = response.InitialWeight,
            };

            return Result<ClientDto>.Success(clientDto);
        }
        catch (Exception ex)
        {
            return Result<ClientDto>.Failure($"Failed to get client: {ex.Message}");
        }
    }
}
