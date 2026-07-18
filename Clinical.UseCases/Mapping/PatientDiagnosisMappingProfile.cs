using AutoMapper;
using Clinical.Application.DTOS.PatientDiagnosis.Response;
using Clinical.Domain.Entities;
using Clinical.UseCases.UseCases.PatientDiagnosis.Commands.ChangeStateCommand;
using Clinical.UseCases.UseCases.PatientDiagnosis.Commands.CreateCommand;

namespace Clinical.UseCases.Mapping;

public class PatientDiagnosisMappingProfile : Profile
{
    public PatientDiagnosisMappingProfile()
    {
        CreateMap<CreateDiagnosisCommand, PatientDiagnosis>();
        CreateMap<ChangeStateDiagnosisCommand, PatientDiagnosis>();
        CreateMap<PatientDiagnosis, GetAllDiagnosisResponseDto>();
    }
}
