using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Doctor.Commands.DeleteCommand;

public class DeleteDoctorCommand : IRequest<BaseResponse<bool>>
{
    public int DoctorId { get; set; }
}
