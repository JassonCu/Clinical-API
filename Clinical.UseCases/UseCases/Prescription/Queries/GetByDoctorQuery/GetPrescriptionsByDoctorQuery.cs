using Clinical.Application.DTOS.Prescription.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Prescription.Queries.GetByDoctorQuery
{
    public class GetPrescriptionsByDoctorQuery : IRequest<BaseResponse<IEnumerable<GetAllPrescriptionResponseDto>>>
    {
        public int DoctorId { get; set; }
    }
}
