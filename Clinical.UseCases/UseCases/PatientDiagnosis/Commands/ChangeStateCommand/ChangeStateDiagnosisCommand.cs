using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.PatientDiagnosis.Commands.ChangeStateCommand
{
    public class ChangeStateDiagnosisCommand : IRequest<BaseResponse<bool>>
    {
        public int? DiagnosisId { get; set; }
        public int? State { get; set; }
    }
}
