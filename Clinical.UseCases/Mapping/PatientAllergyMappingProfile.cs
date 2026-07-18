using AutoMapper;
using Clinical.Application.DTOS.PatientAllergy.Response;
using Clinical.Domain.Entities;
using Clinical.UseCases.UseCases.PatientAllergy.Commands.ChangeStateCommand;
using Clinical.UseCases.UseCases.PatientAllergy.Commands.CreateCommand;
using Clinical.UseCases.UseCases.PatientAllergy.Commands.UpdateCommand;

namespace Clinical.UseCases.Mapping;

public class PatientAllergyMappingProfile : Profile
{
    public PatientAllergyMappingProfile()
    {
        CreateMap<CreateAllergyCommand, PatientAllergy>();
        CreateMap<UpdateAllergyCommand, PatientAllergy>();
        CreateMap<ChangeStateAllergyCommand, PatientAllergy>();
        CreateMap<PatientAllergy, GetAllAllergyResponseDto>();
    }
}
