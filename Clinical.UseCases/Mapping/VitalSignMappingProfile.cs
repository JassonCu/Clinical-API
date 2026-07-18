using AutoMapper;
using Clinical.Application.DTOS.VitalSign.Response;
using Clinical.Domain.Entities;
using Clinical.UseCases.UseCases.VitalSign.Commands.CreateCommand;

namespace Clinical.UseCases.Mapping;

public class VitalSignMappingProfile : Profile
{
    public VitalSignMappingProfile()
    {
        CreateMap<CreateVitalSignCommand, VitalSign>();
        CreateMap<VitalSign, GetAllVitalSignResponseDto>();
    }
}
