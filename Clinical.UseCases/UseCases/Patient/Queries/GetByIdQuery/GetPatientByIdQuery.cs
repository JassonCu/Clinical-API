using Clinical.Application.DTOS.Patient.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Patient.Queries.GetByIdQuery;

public class GetPatientByIdQuery : IRequest<BaseResponse<GetPatientByIdResponseDto>>
{
    public int PatientId { get; set; }
}
