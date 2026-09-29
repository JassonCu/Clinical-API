using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;
using Clinical.UseCases.Commons.Exceptions;

namespace Clinical.UseCases.UseCases.Appointment.Commands.UpdateCommand;

public class UpdateAppointmentHandler : IRequestHandler<UpdateAppointmentCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAppointmentConflictChecker _conflictChecker;

    public UpdateAppointmentHandler(IUnitOfWork unitOfWork, IMapper mapper, IAppointmentConflictChecker conflictChecker)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _conflictChecker = conflictChecker;
    }

    public async Task<BaseResponse<bool>> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        // Rescheduling: the command carries no DoctorId, so resolve it from the stored appointment
        // and check the doctor's agenda for a conflict, excluding this same appointment.
        if (request.AppointmentDate is DateTime when)
        {
            var existing = await _unitOfWork.Appointment.GetByIdAsync(
                StoreProcedures.uspAppointmentById, new { AppointmentId = request.AppointmentId });

            if (existing?.DoctorId is int doctorId)
            {
                var conflict = await _conflictChecker.FindConflictAsync(doctorId, when, request.AppointmentId);
                if (conflict is not null)
                    throw new ConflictException(
                        $"El médico ya tiene una cita agendada el {conflict.AppointmentDate:dd/MM/yyyy HH:mm}. Elija otro horario.");
            }
        }

        var appointment = _mapper.Map<Entity.Appointment>(request);
        var parameters = appointment.GetPropertiesWithValues();
        response.Data = await _unitOfWork.Appointment.ExecAsync(StoreProcedures.uspAppointmentEdit, parameters);

        if (!response.Data)
            throw new NotFoundException(GlobalMessage.MESSAGE_QUERY_EMPTY);

        response.IsSuccess = true;
        response.Message = GlobalMessage.MESSAGE_UPDATE;

        return response;
    }
}
