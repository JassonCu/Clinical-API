using Clinical.Application.DTOS.Patient.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Patient.Queries.GetAllQuery;

public class GetAllPatientQuery : IRequest<BaseResponse<IEnumerable<GetAllPatientResponseDto>>>
{
}
