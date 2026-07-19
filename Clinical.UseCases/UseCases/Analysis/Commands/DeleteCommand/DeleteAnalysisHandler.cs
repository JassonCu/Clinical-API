using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;
using Clinical.UseCases.Commons.Exceptions;

namespace Clinical.UseCases.UseCases.Analysis.Commands.DeleteCommand;

public class DeleteAnalysisHandler : IRequestHandler<DeleteAnalysisCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAnalysisHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseResponse<bool>> Handle(DeleteAnalysisCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        response.Data = await _unitOfWork.Analysis.ExecAsync(StoreProcedures.uspAnalysisRemove, request);

        if (!response.Data)
            throw new NotFoundException(GlobalMessage.MESSAGE_QUERY_EMPTY);

        response.IsSuccess = true;
        response.Message = GlobalMessage.MESSAGE_DELETE;

        return response;
    }
}
