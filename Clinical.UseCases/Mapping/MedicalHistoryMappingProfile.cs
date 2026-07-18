using AutoMapper;
using Clinical.Application.DTOS.MedicalHistory.Response;
using Clinical.Domain.Entities;
using Clinical.UseCases.UseCases.MedicalHistory.Commands.CreateCommand;
using Clinical.UseCases.UseCases.MedicalHistory.Commands.UpdateCommand;

namespace Clinical.UseCases.Mapping;

public class MedicalHistoryMappingProfile : Profile
{
    public MedicalHistoryMappingProfile()
    {
        CreateMap<CreateMedicalHistoryCommand, MedicalHistory>();
        CreateMap<UpdateMedicalHistoryCommand, MedicalHistory>();
        CreateMap<MedicalHistory, GetMedicalHistoryResponseDto>();
    }
}
