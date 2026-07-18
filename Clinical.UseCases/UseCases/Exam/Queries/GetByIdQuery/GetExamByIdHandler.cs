using AutoMapper;
using Clinical.Application.DTOS.Exam.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;
using Clinical.UseCases.Commons.Exceptions;

namespace Clinical.UseCases.UseCases.Exam.Queries.GetByIdQuery;

public class GetExamByIdHandler : IRequestHandler<GetExamByIdQuery, BaseResponse<GetExamByIdResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetExamByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetExamByIdResponseDto>> Handle(GetExamByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<GetExamByIdResponseDto>();

        var exam = await _unitOfWork.Exam.GetByIdAsync(StoreProcedures.uspExamById, request);

        if (exam is null) throw new NotFoundException(GlobalMessage.MESSAGE_QUERY_EMPTY);

        response.IsSuccess = true;
        response.Data = _mapper.Map<GetExamByIdResponseDto>(exam);
        response.Message = GlobalMessage.MESSAGE_QUERY;

        return response;
    }
}
