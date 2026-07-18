using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.Doctor.Commands.CreateCommand;

public class CreateDoctorHandler : IRequestHandler<CreateDoctorCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateDoctorHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<bool>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var doctor = _mapper.Map<Entity.Doctor>(request);
            var parameters = doctor.GetPropertiesWithValues();
            response.Data = await _unitOfWork.Doctor.ExecAsync(StoreProcedures.uspDoctorRegister, parameters);

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = GlobalMessage.MESSAGE_SAVE;
            }
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
