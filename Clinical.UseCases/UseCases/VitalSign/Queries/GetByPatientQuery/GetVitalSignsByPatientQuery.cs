using Clinical.Application.DTOS.VitalSign.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.VitalSign.Queries.GetByPatientQuery
{
    public class GetVitalSignsByPatientQuery : IRequest<BaseResponse<IEnumerable<GetAllVitalSignResponseDto>>>
    {
        public int PatientId { get; set; }
    }
}
