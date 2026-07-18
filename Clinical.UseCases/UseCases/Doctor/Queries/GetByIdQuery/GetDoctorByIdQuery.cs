using Clinical.Application.DTOS.Doctor.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Doctor.Queries.GetByIdQuery;

public class GetDoctorByIdQuery : IRequest<BaseResponse<GetDoctorByIdResponseDto>>
{
    public int DoctorId { get; set; }
}
