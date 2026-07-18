using AutoMapper;
using Clinical.Application.DTOS.MedicalHistory.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;
using Clinical.UseCases.Commons.Exceptions;

namespace Clinical.UseCases.UseCases.MedicalHistory.Queries.GetByPatientQuery;

public class GetMedicalHistoryByPatientHandler : IRequestHandler<GetMedicalHistoryByPatientQuery, BaseResponse<GetMedicalHistoryResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMedicalHistoryByPatientHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetMedicalHistoryResponseDto>> Handle(GetMedicalHistoryByPatientQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<GetMedicalHistoryResponseDto>();

        var entity = await _unitOfWork.MedicalHistory.GetByIdAsync(StoreProcedures.uspMedicalHistoryByPatient, request);
        if (entity is null) throw new NotFoundException(GlobalMessage.MESSAGE_QUERY_EMPTY);
        response.IsSuccess = true;
        response.Data = _mapper.Map<GetMedicalHistoryResponseDto>(entity);
        response.Message = GlobalMessage.MESSAGE_QUERY;

        return response;
    }
}
