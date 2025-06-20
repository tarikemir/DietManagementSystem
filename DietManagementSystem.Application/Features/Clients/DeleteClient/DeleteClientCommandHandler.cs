using DietManagementSystem.Application.Common;
using MediatR;

namespace DietManagementSystem.Application.Features.Clients.DeleteClient;

public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteClientCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException("UnitOfWork is null");
    }

    public async Task<Result> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.Clients.DeleteAsync(request.Id);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Couldn't delete the client => {ex.Message}");
        }
    }
}
