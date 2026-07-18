using AutoMapper;
using Clinical.Application.DTOS.MedicalHistory.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

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
        try
        {
            var entity = await _unitOfWork.MedicalHistory.GetByIdAsync(StoreProcedures.uspMedicalHistoryById, request);
            if (entity is null) { response.IsSuccess = false; response.Message = GlobalMessage.MESSAGE_QUERY_EMPTY; return response; }
            response.IsSuccess = true;
            response.Data = _mapper.Map<GetMedicalHistoryResponseDto>(entity);
            response.Message = GlobalMessage.MESSAGE_QUERY;
        }
        catch (Exception ex) { response.Message = ex.Message; }
        return response;
    }
}
