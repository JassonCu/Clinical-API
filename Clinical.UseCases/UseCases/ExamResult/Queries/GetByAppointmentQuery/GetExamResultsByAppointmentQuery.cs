using Clinical.Application.DTOS.ExamResult.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.ExamResult.Queries.GetByAppointmentQuery;

public class GetExamResultsByAppointmentQuery : IRequest<BaseResponse<IEnumerable<GetAllExamResultResponseDto>>>
{
    public int AppointmentId { get; set; }
}
