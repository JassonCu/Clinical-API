using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Patient.Commands.DeleteCommand;

public class DeletePatientCommand : IRequest<BaseResponse<bool>>
{
    public int PatientId { get; set; }
}
