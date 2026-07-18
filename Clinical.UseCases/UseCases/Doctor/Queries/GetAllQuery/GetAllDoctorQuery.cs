using Clinical.Application.DTOS.Doctor.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Doctor.Queries.GetAllQuery;

public class GetAllDoctorQuery : IRequest<BaseResponse<IEnumerable<GetAllDoctorResponseDto>>>
{
}
