using AutoMapper;
using Clinical.Application.DTOS.Patient.Response;
using Clinical.Domain.Entities;
using Clinical.UseCases.UseCases.Patient.Commands.ChangeStateCommand;
using Clinical.UseCases.UseCases.Patient.Commands.CreateCommand;
using Clinical.UseCases.UseCases.Patient.Commands.UpdateCommand;

namespace Clinical.UseCases.Mapping;

public class PatientMappingProfile : Profile
{
    public PatientMappingProfile()
    {
        CreateMap<Patient, GetAllPatientResponseDto>().ReverseMap();
        CreateMap<Patient, GetPatientByIdResponseDto>().ReverseMap();

        CreateMap<CreatePatientCommand, Patient>();
        CreateMap<UpdatePatientCommand, Patient>();
        CreateMap<ChangeStatePatientCommand, Patient>();
    }
}
