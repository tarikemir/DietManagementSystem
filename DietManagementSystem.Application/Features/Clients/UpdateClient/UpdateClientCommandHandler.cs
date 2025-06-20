using AutoMapper;
using DietManagementSystem.Application.Common;
using DietManagementSystem.Application.Dtos;
using MediatR;

namespace DietManagementSystem.Application.Features.Clients.UpdateClient;

public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, Result<ClientDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<ClientDto>> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(request.Id);
            if (client == null)
                return Result<ClientDto>.Failure("Client not found.");

            client.FirstName = request.FirstName;
            client.LastName = request.LastName;
            client.Email = request.Email;
            client.PhoneNumber = request.PhoneNumber;
            client.InitialWeight = request.InitialWeight;

            await _unitOfWork.Clients.UpdateAsync(client);
            await _unitOfWork.SaveChangesAsync();

            var clientDto = _mapper.Map<ClientDto>(client);

            return Result<ClientDto>.Success(clientDto);


        }
        catch (Exception ex)
        {
            return Result<ClientDto>.Failure($"Client cannot be updated => {ex.Message}");
        }
    }
}
