using DietManagementSystem.Application.Dtos;
using FluentValidation;

namespace DietManagementSystem.Application.Features.Clients.GetAllClients;

public class GetAllClientsQueryValidator : AbstractValidator<GetAllClientsQuery>
{
    public GetAllClientsQueryValidator()
    {
        RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.Size)
                .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
    }
}
