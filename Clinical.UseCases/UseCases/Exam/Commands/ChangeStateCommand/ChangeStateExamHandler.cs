using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.Exam.Commands.ChangeStateCommand;

public class ChangeStateExamHandler : IRequestHandler<ChangeStateExamCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ChangeStateExamHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<bool>> Handle(ChangeStateExamCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        var exam = _mapper.Map<Entity.Exam>(request);
        var parameters = exam.GetPropertiesWithValues();
        response.Data = await _unitOfWork.Exam.ExecAsync(StoreProcedures.uspExamChangeState, parameters);

        if (response.Data)
        {
            response.IsSuccess = true;
            response.Message = GlobalMessage.MESSAGE_UPDATE_STATE;
        }

        return response;
    }
}
