using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.MedicalHistory.Commands.UpdateCommand;

public class UpdateMedicalHistoryHandler : IRequestHandler<UpdateMedicalHistoryCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateMedicalHistoryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<bool>> Handle(UpdateMedicalHistoryCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        var entity = _mapper.Map<Entity.MedicalHistory>(request);
        var parameters = entity.GetPropertiesWithValues();
        response.Data = await _unitOfWork.MedicalHistory.ExecAsync(StoreProcedures.uspMedicalHistoryEdit, parameters);
        if (response.Data) { response.IsSuccess = true; response.Message = GlobalMessage.MESSAGE_UPDATE; }

        return response;
    }
}
