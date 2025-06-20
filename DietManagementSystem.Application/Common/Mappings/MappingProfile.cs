using DietManagementSystem.Application.Dtos;
using DietManagementSystem.Application.Features.Clients.UpdateClient;
using DietManagementSystem.Domain.Entities;
using AutoMapper;

namespace DietManagementSystem.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UpdateClientCommand, Client>();
        CreateMap<Client, ClientDto>();
    }
}
