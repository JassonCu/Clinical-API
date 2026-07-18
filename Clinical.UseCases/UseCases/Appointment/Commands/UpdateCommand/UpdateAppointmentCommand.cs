using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Appointment.Commands.UpdateCommand;

public class UpdateAppointmentCommand : IRequest<BaseResponse<bool>>
{
    public int AppointmentId { get; set; }
    public DateTime? AppointmentDate { get; set; }
    public string? Reason { get; set; }
    public string? Diagnosis { get; set; }
    public string? Notes { get; set; }
}
