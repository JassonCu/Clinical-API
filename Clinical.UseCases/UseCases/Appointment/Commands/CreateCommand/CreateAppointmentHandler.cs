using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.UseCases.Commons.Exceptions;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.Appointment.Commands.CreateCommand;

public class CreateAppointmentHandler : IRequestHandler<CreateAppointmentCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAppointmentConflictChecker _conflictChecker;

    public CreateAppointmentHandler(IUnitOfWork unitOfWork, IMapper mapper, IAppointmentConflictChecker conflictChecker)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _conflictChecker = conflictChecker;
    }

    public async Task<BaseResponse<bool>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        var conflict = await _conflictChecker.FindConflictAsync(request.DoctorId, request.AppointmentDate);
        if (conflict is not null)
            throw new ConflictException(
                $"El médico ya tiene una cita agendada el {conflict.AppointmentDate:dd/MM/yyyy HH:mm}. Elija otro horario.");

        var appointment = _mapper.Map<Entity.Appointment>(request);
        var parameters = appointment.GetPropertiesWithValues();
        response.Data = await _unitOfWork.Appointment.ExecAsync(StoreProcedures.uspAppointmentRegister, parameters);

        if (response.Data)
        {
            response.IsSuccess = true;
            response.Message = GlobalMessage.MESSAGE_SAVE;
        }

        return response;
    }
}
