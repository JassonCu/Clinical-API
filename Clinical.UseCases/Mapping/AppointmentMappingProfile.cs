using AutoMapper;
using Clinical.Domain.Entities;
using Clinical.UseCases.UseCases.Appointment.Commands.ChangeStateCommand;
using Clinical.UseCases.UseCases.Appointment.Commands.CreateCommand;
using Clinical.UseCases.UseCases.Appointment.Commands.UpdateCommand;

namespace Clinical.UseCases.Mapping;

public class AppointmentMappingProfile : Profile
{
    public AppointmentMappingProfile()
    {
        CreateMap<CreateAppointmentCommand, Appointment>();
        CreateMap<UpdateAppointmentCommand, Appointment>();
        CreateMap<ChangeStateAppointmentCommand, Appointment>();
    }
}
