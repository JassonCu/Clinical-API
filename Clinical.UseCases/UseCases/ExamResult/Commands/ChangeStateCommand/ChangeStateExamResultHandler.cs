using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.ExamResult.Commands.ChangeStateCommand;

public class ChangeStateExamResultHandler : IRequestHandler<ChangeStateExamResultCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ChangeStateExamResultHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<bool>> Handle(ChangeStateExamResultCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        var examResult = _mapper.Map<Entity.ExamResult>(request);
        var parameters = examResult.GetPropertiesWithValues();
        response.Data = await _unitOfWork.ExamResult.ExecAsync(StoreProcedures.uspExamResultChangeState, parameters);

        if (response.Data)
        {
            response.IsSuccess = true;
            response.Message = GlobalMessage.MESSAGE_UPDATE_STATE;
        }

        return response;
    }
}