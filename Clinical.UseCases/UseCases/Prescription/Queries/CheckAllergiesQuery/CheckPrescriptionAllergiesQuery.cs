using Clinical.Application.DTOS.Prescription.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Prescription.Queries.CheckAllergiesQuery
{
    /// <summary>Checks the given medicines against a patient's registered allergies before prescribing.</summary>
    public class CheckPrescriptionAllergiesQuery : IRequest<BaseResponse<AllergyCheckResponseDto>>
    {
        public int PatientId { get; set; }
        public List<int> MedicineIds { get; set; } = [];
    }
}
