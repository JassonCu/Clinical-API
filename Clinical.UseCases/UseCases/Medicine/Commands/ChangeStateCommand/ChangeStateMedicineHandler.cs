using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.Medicine.Commands.ChangeStateCommand;

public class ChangeStateMedicineHandler : IRequestHandler<ChangeStateMedicineCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ChangeStateMedicineHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<bool>> Handle(ChangeStateMedicineCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        var entity = _mapper.Map<Entity.Medicine>(request);
        var parameters = entity.GetPropertiesWithValues();
        response.Data = await _unitOfWork.Medicine.ExecAsync(StoreProcedures.uspMedicineChangeState, parameters);
        if (response.Data) { response.IsSuccess = true; response.Message = GlobalMessage.MESSAGE_UPDATE_STATE; }

        return response;
    }
}
