using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Appointment.Commands.ChangeStateCommand;

public class ChangeStateAppointmentCommand : IRequest<BaseResponse<bool>>
{
    public int AppointmentId { get; set; }
    public int State { get; set; }
}
