using Clinical.Application.DTOS.ExamResult.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.ExamResult.Queries.GetByPatientQuery;

public class GetExamResultsByPatientQuery : IRequest<BaseResponse<IEnumerable<GetAllExamResultResponseDto>>>
{
    public int PatientId { get; set; }
}
