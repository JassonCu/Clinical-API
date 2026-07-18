using Clinical.Application.DTOS.ExamResult.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.ExamResult.Queries.GetByIdQuery;

public class GetExamResultByIdHandler : IRequestHandler<GetExamResultByIdQuery, BaseResponse<GetExamResultByIdResponseDto>>
{
    private readonly IExamResultRepository _examResultRepository;

    public GetExamResultByIdHandler(IExamResultRepository examResultRepository)
    {
        _examResultRepository = examResultRepository;
    }

    public async Task<BaseResponse<GetExamResultByIdResponseDto>> Handle(GetExamResultByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<GetExamResultByIdResponseDto>();

        try
        {
            var result = await _examResultRepository.GetExamResultById(StoreProcedures.uspExamResultById, request);

            if (result is null)
            {
                response.IsSuccess = false;
                response.Message = GlobalMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.IsSuccess = true;
            response.Data = result;
            response.Message = GlobalMessage.MESSAGE_QUERY;
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}