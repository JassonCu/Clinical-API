using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.ExamResult.Commands.ChangeStateCommand;

public class ChangeStateExamResultCommand : IRequest<BaseResponse<bool>>
{
    public int ExamResultId { get; set; }
    public int State { get; set; }
}
