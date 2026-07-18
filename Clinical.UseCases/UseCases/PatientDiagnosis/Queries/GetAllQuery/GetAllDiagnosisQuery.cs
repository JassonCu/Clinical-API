using Clinical.Application.DTOS.PatientDiagnosis.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.PatientDiagnosis.Queries.GetAllQuery
{
    public class GetAllDiagnosisQuery : IRequest<BaseResponse<IEnumerable<GetAllDiagnosisResponseDto>>>
    {
    }
}
