using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.ExamResult.Commands.DeleteCommand;

public class DeleteExamResultCommand : IRequest<BaseResponse<bool>>
{
    public int ExamResultId { get; set; }
}
