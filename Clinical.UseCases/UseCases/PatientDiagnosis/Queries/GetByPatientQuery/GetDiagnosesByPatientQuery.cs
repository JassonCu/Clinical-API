using Clinical.Application.DTOS.PatientDiagnosis.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.PatientDiagnosis.Queries.GetByPatientQuery
{
    public class GetDiagnosesByPatientQuery : IRequest<BaseResponse<IEnumerable<GetAllDiagnosisResponseDto>>>
    {
        public int PatientId { get; set; }
    }
}
