using Clinical.Application.DTOS.Prescription.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Prescription.Queries.GetByPatientQuery
{
    public class GetPrescriptionsByPatientQuery : IRequest<BaseResponse<IEnumerable<GetAllPrescriptionResponseDto>>>
    {
        public int PatientId { get; set; }
    }
}
