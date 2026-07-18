using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Patient.Commands.ChangeStateCommand;

public class ChangeStatePatientCommand : IRequest<BaseResponse<bool>>
{
    public int PatientId { get; set; }
    public int State { get; set; }
}
