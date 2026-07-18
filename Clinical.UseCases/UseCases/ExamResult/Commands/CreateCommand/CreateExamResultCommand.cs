using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.ExamResult.Commands.CreateCommand;

public class CreateExamResultCommand : IRequest<BaseResponse<bool>>
{
    public int PatientId { get; set; }
    public int ExamId { get; set; }
    public int? AppointmentId { get; set; }
    public string? Result { get; set; }
    public string? Observations { get; set; }
    public DateTime ResultDate { get; set; }
}
