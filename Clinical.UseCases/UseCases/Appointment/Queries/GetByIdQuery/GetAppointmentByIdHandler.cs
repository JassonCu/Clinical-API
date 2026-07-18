using Clinical.Application.DTOS.Appointment.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;
using Clinical.UseCases.Commons.Exceptions;

namespace Clinical.UseCases.UseCases.Appointment.Queries.GetByIdQuery;

public class GetAppointmentByIdHandler : IRequestHandler<GetAppointmentByIdQuery, BaseResponse<GetAppointmentByIdResponseDto>>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public GetAppointmentByIdHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<BaseResponse<GetAppointmentByIdResponseDto>> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<GetAppointmentByIdResponseDto>();

        var appointment = await _appointmentRepository.GetAppointmentById(StoreProcedures.uspAppointmentById, request);

        if (appointment is null) throw new NotFoundException(GlobalMessage.MESSAGE_QUERY_EMPTY);

        response.IsSuccess = true;
        response.Data = appointment;
        response.Message = GlobalMessage.MESSAGE_QUERY;

        return response;
    }
}
