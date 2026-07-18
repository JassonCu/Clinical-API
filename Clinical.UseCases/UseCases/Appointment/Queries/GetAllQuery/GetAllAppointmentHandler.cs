using Clinical.Application.DTOS.Appointment.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Appointment.Queries.GetAllQuery
{
    public class GetAllAppointmentHandler : IRequestHandler<GetAllAppointmentQuery, BaseResponse<IEnumerable<GetAllAppointmentResponseDto>>>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public GetAllAppointmentHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<BaseResponse<IEnumerable<GetAllAppointmentResponseDto>>> Handle(GetAllAppointmentQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<GetAllAppointmentResponseDto>>();

            try
            {
                var appointments = await _appointmentRepository.GetAllAppointments(StoreProcedures.uspAppointmentList);

                if (appointments is not null)
                {
                    response.IsSuccess = true;
                    response.Data = appointments;
                    response.Message = GlobalMessage.MESSAGE_QUERY;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
