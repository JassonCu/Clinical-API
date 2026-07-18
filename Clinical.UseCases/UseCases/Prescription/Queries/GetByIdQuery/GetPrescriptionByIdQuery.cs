using Clinical.Application.DTOS.Prescription.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Prescription.Queries.GetByIdQuery
{
    public class GetPrescriptionByIdQuery : IRequest<BaseResponse<GetPrescriptionByIdResponseDto>>
    {
        public int PrescriptionId { get; set; }
    }
}
