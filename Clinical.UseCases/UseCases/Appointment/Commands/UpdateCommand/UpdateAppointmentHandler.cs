using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.Appointment.Commands.UpdateCommand;

public class UpdateAppointmentHandler : IRequestHandler<UpdateAppointmentCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateAppointmentHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<bool>> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        var appointment = _mapper.Map<Entity.Appointment>(request);
        var parameters = appointment.GetPropertiesWithValues();
        response.Data = await _unitOfWork.Appointment.ExecAsync(StoreProcedures.uspAppointmentEdit, parameters);

        if (response.Data)
        {
            response.IsSuccess = true;
            response.Message = GlobalMessage.MESSAGE_UPDATE;
        }

        return response;
    }
}
