using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.Appointment.Commands.ChangeStateCommand;

public class ChangeStateAppointmentHandler : IRequestHandler<ChangeStateAppointmentCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ChangeStateAppointmentHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<bool>> Handle(ChangeStateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        var appointment = _mapper.Map<Entity.Appointment>(request);
        var parameters = appointment.GetPropertiesWithValues();
        response.Data = await _unitOfWork.Appointment.ExecAsync(StoreProcedures.uspAppointmentChangeState, parameters);

        if (response.Data)
        {
            response.IsSuccess = true;
            response.Message = GlobalMessage.MESSAGE_UPDATE_STATE;
        }

        return response;
    }
}
