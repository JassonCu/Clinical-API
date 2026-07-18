using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.Doctor.Commands.ChangeStateCommand;

public class ChangeStateDoctorHandler : IRequestHandler<ChangeStateDoctorCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ChangeStateDoctorHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<bool>> Handle(ChangeStateDoctorCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        var doctor = _mapper.Map<Entity.Doctor>(request);
        var parameters = doctor.GetPropertiesWithValues();
        response.Data = await _unitOfWork.Doctor.ExecAsync(StoreProcedures.uspDoctorChangeState, parameters);

        if (response.Data)
        {
            response.IsSuccess = true;
            response.Message = GlobalMessage.MESSAGE_UPDATE_STATE;
        }

        return response;
    }
}
