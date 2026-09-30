using Clinical.Application.DTOS.Patient.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Patient.Queries.GetSummaryQuery
{
    /// <summary>Returns the consolidated 360° clinical summary for a patient.</summary>
    public class GetPatientSummaryQuery : IRequest<BaseResponse<PatientSummaryDto>>
    {
        public int PatientId { get; set; }
    }
}
