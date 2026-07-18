using Clinical.Application.DTOS.ExamResult.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.ExamResult.Queries.GetAllQuery;

public class GetAllExamResultQuery : IRequest<BaseResponse<IEnumerable<GetAllExamResultResponseDto>>>
{
}
