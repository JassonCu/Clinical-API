using Clinical.Application.DTOS.Appointment.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Appointment.Queries.GetByIdQuery
{
    public class GetAppointmentByIdQuery : IRequest<BaseResponse<GetAppointmentByIdResponseDto>>
    {
        public int AppointmentId { get; set; }
    }
}
