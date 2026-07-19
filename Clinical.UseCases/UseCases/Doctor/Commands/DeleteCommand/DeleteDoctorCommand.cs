using MediatR;

namespace Clinical.UseCases.UseCases.Doctor.Commands.DeleteCommand;

public class DeleteDoctorCommand : IRequest
{
    public int DoctorId { get; set; }
}
