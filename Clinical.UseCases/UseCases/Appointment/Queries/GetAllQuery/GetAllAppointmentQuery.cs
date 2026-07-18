using Clinical.Application.DTOS.Appointment.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Appointment.Queries.GetAllQuery
{
    public class GetAllAppointmentQuery : IRequest<BaseResponse<IEnumerable<GetAllAppointmentResponseDto>>>
    {
    }
}
