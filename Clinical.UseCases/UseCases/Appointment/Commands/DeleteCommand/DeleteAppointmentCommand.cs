using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Appointment.Commands.DeleteCommand;

public class DeleteAppointmentCommand : IRequest<BaseResponse<bool>>
{
    public int AppointmentId { get; set; }
}
