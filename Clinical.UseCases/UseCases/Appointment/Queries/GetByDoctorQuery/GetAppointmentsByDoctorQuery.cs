using Clinical.Application.DTOS.Appointment.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Appointment.Queries.GetByDoctorQuery
{
    public class GetAppointmentsByDoctorQuery : IRequest<BaseResponse<IEnumerable<GetAllAppointmentResponseDto>>>
    {
        public int DoctorId { get; set; }
    }
}
