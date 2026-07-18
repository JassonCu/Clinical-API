using AutoMapper;
using Clinical.Application.DTOS.Doctor.Response;
using Clinical.Domain.Entities;
using Clinical.UseCases.UseCases.Doctor.Commands.ChangeStateCommand;
using Clinical.UseCases.UseCases.Doctor.Commands.CreateCommand;
using Clinical.UseCases.UseCases.Doctor.Commands.UpdateCommand;

namespace Clinical.UseCases.Mapping;

public class DoctorMappingProfile : Profile
{
    public DoctorMappingProfile()
    {
        CreateMap<Doctor, GetAllDoctorResponseDto>().ReverseMap();
        CreateMap<Doctor, GetDoctorByIdResponseDto>().ReverseMap();

        CreateMap<CreateDoctorCommand, Doctor>();
        CreateMap<UpdateDoctorCommand, Doctor>();
        CreateMap<ChangeStateDoctorCommand, Doctor>();
    }
}
