using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.Medicine.Commands.CreateCommand;

public class CreateMedicineHandler : IRequestHandler<CreateMedicineCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateMedicineHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<bool>> Handle(CreateMedicineCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var entity = _mapper.Map<Entity.Medicine>(request);
            entity.State = 1;
            var parameters = entity.GetPropertiesWithValues();
            response.Data = await _unitOfWork.Medicine.ExecAsync(StoreProcedures.uspMedicineRegister, parameters);
            if (response.Data) { response.IsSuccess = true; response.Message = GlobalMessage.MESSAGE_SAVE; }
        }
        catch (Exception ex) { response.Message = ex.Message; }
        return response;
    }
}
