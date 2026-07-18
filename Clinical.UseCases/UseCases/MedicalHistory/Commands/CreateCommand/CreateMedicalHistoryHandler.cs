using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.MedicalHistory.Commands.CreateCommand;

public class CreateMedicalHistoryHandler : IRequestHandler<CreateMedicalHistoryCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateMedicalHistoryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<bool>> Handle(CreateMedicalHistoryCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        var entity = _mapper.Map<Entity.MedicalHistory>(request);
        entity.State = 1;
        var parameters = entity.GetPropertiesWithValues();
        response.Data = await _unitOfWork.MedicalHistory.ExecAsync(StoreProcedures.uspMedicalHistoryRegister, parameters);
        if (response.Data) { response.IsSuccess = true; response.Message = GlobalMessage.MESSAGE_SAVE; }

        return response;
    }
}
