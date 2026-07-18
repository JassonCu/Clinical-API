using AutoMapper;
using Entity = Clinical.Domain.Entities;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using MediatR;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;

namespace Clinical.UseCases.UseCases.Analysis.Commands.ChangeStateCommand;

public class ChangeStateAnalysisHandler : IRequestHandler<ChangeStateAnalysisCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ChangeStateAnalysisHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<bool>> Handle(ChangeStateAnalysisCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        var analysis = _mapper.Map<Entity.Analysis>(request);
        var parameters = analysis.GetPropertiesWithValues();
        response.Data = await _unitOfWork.Analysis.ExecAsync(StoreProcedures.uspAnalysisChangeState, parameters);

        if (response.Data)
        {
            response.IsSuccess = true;
            response.Message = GlobalMessage.MESSAGE_UPDATE;
        }

        return response;
    }
}
