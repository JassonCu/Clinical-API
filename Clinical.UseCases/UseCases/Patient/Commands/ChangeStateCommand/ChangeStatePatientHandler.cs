using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.Patient.Commands.ChangeStateCommand;

public class ChangeStatePatientHandler : IRequestHandler<ChangeStatePatientCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ChangeStatePatientHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<bool>> Handle(ChangeStatePatientCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var patient = _mapper.Map<Entity.Patient>(request);
            var parameters = patient.GetPropertiesWithValues();
            response.Data = await _unitOfWork.Patient.ExecAsync(StoreProcedures.uspPatientChangeState, parameters);

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = GlobalMessage.MESSAGE_UPDATE_STATE;
            }
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
