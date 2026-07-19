using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;
using Clinical.UseCases.Commons.Exceptions;

namespace Clinical.UseCases.UseCases.Exam.Commands.DeleteCommand;

public class DeleteExamHandler : IRequestHandler<DeleteExamCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteExamHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseResponse<bool>> Handle(DeleteExamCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        response.Data = await _unitOfWork.Exam.ExecAsync(StoreProcedures.uspExamRemove, request);

        if (!response.Data)
            throw new NotFoundException(GlobalMessage.MESSAGE_QUERY_EMPTY);

        response.IsSuccess = true;
        response.Message = GlobalMessage.MESSAGE_DELETE;

        return response;
    }
}