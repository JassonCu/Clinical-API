using MediatR;

namespace Clinical.UseCases.UseCases.Doctor.Commands.ChangeStateCommand;

public class ChangeStateDoctorCommand : IRequest
{
    public int DoctorId { get; set; }
    public int State { get; set; }
}
