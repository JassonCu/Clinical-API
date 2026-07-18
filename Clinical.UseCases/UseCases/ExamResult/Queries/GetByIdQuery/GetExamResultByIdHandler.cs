using Clinical.Application.DTOS.ExamResult.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;
using Clinical.UseCases.Commons.Exceptions;

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

        var result = await _examResultRepository.GetExamResultById(StoreProcedures.uspExamResultById, request);

        if (result is null) throw new NotFoundException(GlobalMessage.MESSAGE_QUERY_EMPTY);

        response.IsSuccess = true;
        response.Data = result;
        response.Message = GlobalMessage.MESSAGE_QUERY;

        return response;
    }
}