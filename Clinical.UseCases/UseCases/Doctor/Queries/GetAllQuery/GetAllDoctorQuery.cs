using Clinical.Application.DTOS.Doctor.Response;
using MediatR;

namespace Clinical.UseCases.UseCases.Doctor.Queries.GetAllQuery;

public class GetAllDoctorQuery : IRequest<IEnumerable<GetAllDoctorResponseDto>>
{
}
