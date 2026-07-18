using Clinical.Application.DTOS.PatientDiagnosis.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.PatientDiagnosis.Queries.GetByAppointmentQuery
{
    public class GetDiagnosesByAppointmentQuery : IRequest<BaseResponse<IEnumerable<GetAllDiagnosisResponseDto>>>
    {
        public int AppointmentId { get; set; }
    }
}
