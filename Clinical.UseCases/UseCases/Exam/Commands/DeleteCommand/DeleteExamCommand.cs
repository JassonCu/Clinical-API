using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Exam.Commands.DeleteCommand;

public class DeleteExamCommand : IRequest<BaseResponse<bool>>
{
    public int ExamId { get; set; }
}
