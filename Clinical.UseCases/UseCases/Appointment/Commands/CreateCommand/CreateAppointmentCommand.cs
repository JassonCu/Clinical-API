using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Appointment.Commands.CreateCommand;

public class CreateAppointmentCommand : IRequest<BaseResponse<bool>>
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string? Reason { get; set; }
    public string? Notes { get; set; }
}
