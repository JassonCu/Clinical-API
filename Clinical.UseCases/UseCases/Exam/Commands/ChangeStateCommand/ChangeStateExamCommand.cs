using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Exam.Commands.ChangeStateCommand;

public class ChangeStateExamCommand : IRequest<BaseResponse<bool>>
{
    public int ExamId { get; set; }
    public int State { get; set; }
}
