using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Doctor.Commands.ChangeStateCommand;

public class ChangeStateDoctorCommand : IRequest<BaseResponse<bool>>
{
    public int DoctorId { get; set; }
    public int State { get; set; }
}
