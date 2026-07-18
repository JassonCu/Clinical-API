using Clinical.Application.DTOS.ExamResult.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.ExamResult.Queries.GetByAppointmentQuery;

public class GetExamResultsByAppointmentHandler : IRequestHandler<GetExamResultsByAppointmentQuery, BaseResponse<IEnumerable<GetAllExamResultResponseDto>>>
{
    private readonly IExamResultRepository _examResultRepository;

    public GetExamResultsByAppointmentHandler(IExamResultRepository examResultRepository)
    {
        _examResultRepository = examResultRepository;
    }

    public async Task<BaseResponse<IEnumerable<GetAllExamResultResponseDto>>> Handle(GetExamResultsByAppointmentQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<GetAllExamResultResponseDto>>();

        try
        {
            var results = await _examResultRepository.GetExamResultsByAppointment(StoreProcedures.uspExamResultByAppointment, request);

            if (results is not null)
            {
                response.IsSuccess = true;
                response.Data = results;
                response.Message = GlobalMessage.MESSAGE_QUERY;
            }
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
