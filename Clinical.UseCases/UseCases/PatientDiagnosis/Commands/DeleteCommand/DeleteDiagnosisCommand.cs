using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.PatientDiagnosis.Commands.DeleteCommand
{
    public class DeleteDiagnosisCommand : IRequest<BaseResponse<bool>>
    {
        public int DiagnosisId { get; set; }
    }
}
