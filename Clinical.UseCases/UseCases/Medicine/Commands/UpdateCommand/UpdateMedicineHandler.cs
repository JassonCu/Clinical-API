using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.Medicine.Commands.UpdateCommand;

public class UpdateMedicineHandler : IRequestHandler<UpdateMedicineCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateMedicineHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<bool>> Handle(UpdateMedicineCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        var entity = _mapper.Map<Entity.Medicine>(request);
        var parameters = entity.GetPropertiesWithValues();
        response.Data = await _unitOfWork.Medicine.ExecAsync(StoreProcedures.uspMedicineEdit, parameters);
        if (response.Data) { response.IsSuccess = true; response.Message = GlobalMessage.MESSAGE_UPDATE; }

        return response;
    }
}
