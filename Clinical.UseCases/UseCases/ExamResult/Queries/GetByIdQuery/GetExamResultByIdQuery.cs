using Clinical.Application.DTOS.ExamResult.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.ExamResult.Queries.GetByIdQuery;

public class GetExamResultByIdQuery : IRequest<BaseResponse<GetExamResultByIdResponseDto>>
{
    public int ExamResultId { get; set; }
}
