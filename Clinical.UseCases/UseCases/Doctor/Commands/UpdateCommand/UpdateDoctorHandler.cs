using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.Doctor.Commands.UpdateCommand;

public class UpdateDoctorHandler : IRequestHandler<UpdateDoctorCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateDoctorHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<bool>> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        var doctor = _mapper.Map<Entity.Doctor>(request);
        var parameters = doctor.GetPropertiesWithValues();
        response.Data = await _unitOfWork.Doctor.ExecAsync(StoreProcedures.uspDoctorEdit, parameters);

        if (response.Data)
        {
            response.IsSuccess = true;
            response.Message = GlobalMessage.MESSAGE_UPDATE;
        }

        return response;
    }
}
