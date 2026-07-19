using Clinical.Application.DTOS.Doctor.Response;
using MediatR;

namespace Clinical.UseCases.UseCases.Doctor.Queries.GetByIdQuery;

public class GetDoctorByIdQuery : IRequest<GetDoctorByIdResponseDto>
{
    public int DoctorId { get; set; }
}
