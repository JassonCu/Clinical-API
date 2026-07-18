using AutoMapper;
using Clinical.Application.DTOS.MedicalHistory.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;
using Clinical.UseCases.Commons.Exceptions;

namespace Clinical.UseCases.UseCases.MedicalHistory.Queries.GetByIdQuery;

public class GetMedicalHistoryByIdHandler : IRequestHandler<GetMedicalHistoryByIdQuery, BaseResponse<GetMedicalHistoryResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMedicalHistoryByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetMedicalHistoryResponseDto>> Handle(GetMedicalHistoryByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<GetMedicalHistoryResponseDto>();

        var entity = await _unitOfWork.MedicalHistory.GetByIdAsync(StoreProcedures.uspMedicalHistoryById, request);
        if (entity is null) throw new NotFoundException(GlobalMessage.MESSAGE_QUERY_EMPTY);
        response.IsSuccess = true;
        response.Data = _mapper.Map<GetMedicalHistoryResponseDto>(entity);
        response.Message = GlobalMessage.MESSAGE_QUERY;

        return response;
    }
}
