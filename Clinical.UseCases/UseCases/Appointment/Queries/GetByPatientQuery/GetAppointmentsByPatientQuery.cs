using Clinical.Application.DTOS.Appointment.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Appointment.Queries.GetByPatientQuery
{
    public class GetAppointmentsByPatientQuery : IRequest<BaseResponse<IEnumerable<GetAllAppointmentResponseDto>>>
    {
        public int PatientId { get; set; }
    }
}
