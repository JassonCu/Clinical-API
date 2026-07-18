using Clinical.Application.DTOS.PatientDiagnosis.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.PatientDiagnosis.Queries.GetByIdQuery
{
    public class GetDiagnosisByIdQuery : IRequest<BaseResponse<GetAllDiagnosisResponseDto>>
    {
        public int DiagnosisId { get; set; }
    }
}
