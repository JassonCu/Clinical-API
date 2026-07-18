using Clinical.Application.DTOS.ExamResult.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.ExamResult.Queries.GetAllQuery;

public class GetAllExamResultHandler : IRequestHandler<GetAllExamResultQuery, BaseResponse<IEnumerable<GetAllExamResultResponseDto>>>
{
    private readonly IExamResultRepository _examResultRepository;

    public GetAllExamResultHandler(IExamResultRepository examResultRepository)
    {
        _examResultRepository = examResultRepository;
    }

    public async Task<BaseResponse<IEnumerable<GetAllExamResultResponseDto>>> Handle(GetAllExamResultQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<GetAllExamResultResponseDto>>();

        try
        {
            var results = await _examResultRepository.GetAllExamResults(StoreProcedures.uspExamResultList);

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