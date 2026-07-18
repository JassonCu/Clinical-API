using Clinical.Application.DTOS.ExamResult.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.ExamResult.Queries.GetByPatientQuery;

public class GetExamResultsByPatientHandler : IRequestHandler<GetExamResultsByPatientQuery, BaseResponse<IEnumerable<GetAllExamResultResponseDto>>>
{
    private readonly IExamResultRepository _examResultRepository;

    public GetExamResultsByPatientHandler(IExamResultRepository examResultRepository)
    {
        _examResultRepository = examResultRepository;
    }

    public async Task<BaseResponse<IEnumerable<GetAllExamResultResponseDto>>> Handle(GetExamResultsByPatientQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<GetAllExamResultResponseDto>>();

        var results = await _examResultRepository.GetExamResultsByPatient(StoreProcedures.uspExamResultByPatient, request);

        if (results is not null)
        {
            response.IsSuccess = true;
            response.Data = results;
            response.Message = GlobalMessage.MESSAGE_QUERY;
        }

        return response;
    }
}
