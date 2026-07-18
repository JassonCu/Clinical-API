using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.ExamResult.Commands.UpdateCommand;

public class UpdateExamResultCommand : IRequest<BaseResponse<bool>>
{
    public int ExamResultId { get; set; }
    public string? Result { get; set; }
    public string? Observations { get; set; }
    public DateTime? ResultDate { get; set; }
}
