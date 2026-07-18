using Clinical.Application.DTOS.Appointment.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Appointment.Queries.GetByDoctorQuery;

public class GetAppointmentsByDoctorHandler : IRequestHandler<GetAppointmentsByDoctorQuery, BaseResponse<IEnumerable<GetAllAppointmentResponseDto>>>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public GetAppointmentsByDoctorHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<BaseResponse<IEnumerable<GetAllAppointmentResponseDto>>> Handle(GetAppointmentsByDoctorQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<GetAllAppointmentResponseDto>>();

        var appointments = await _appointmentRepository.GetAppointmentsByDoctor(StoreProcedures.uspAppointmentByDoctor, request);

        if (appointments is not null)
        {
            response.IsSuccess = true;
            response.Data = appointments;
            response.Message = GlobalMessage.MESSAGE_QUERY;
        }

        return response;
    }
}
