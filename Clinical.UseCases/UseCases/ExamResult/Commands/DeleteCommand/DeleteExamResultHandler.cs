using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.ExamResult.Commands.DeleteCommand;

public class DeleteExamResultHandler : IRequestHandler<DeleteExamResultCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteExamResultHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseResponse<bool>> Handle(DeleteExamResultCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            response.Data = await _unitOfWork.ExamResult.ExecAsync(StoreProcedures.uspExamResultRemove, request);

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = GlobalMessage.MESSAGE_DELETE;
            }
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}