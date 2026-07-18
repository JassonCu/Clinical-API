using AutoMapper;
using Clinical.Application.DTOS.VitalSign.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.VitalSign.Queries.GetByPatientQuery;

public class GetVitalSignsByPatientHandler : IRequestHandler<GetVitalSignsByPatientQuery, BaseResponse<IEnumerable<GetAllVitalSignResponseDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetVitalSignsByPatientHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<IEnumerable<GetAllVitalSignResponseDto>>> Handle(GetVitalSignsByPatientQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<GetAllVitalSignResponseDto>>();

        var entities = await _unitOfWork.VitalSign.GetAllAsync(StoreProcedures.uspVitalSignByPatient, new { PatientId = request.PatientId });
        if (entities is not null) { response.IsSuccess = true; response.Data = _mapper.Map<IEnumerable<GetAllVitalSignResponseDto>>(entities); response.Message = GlobalMessage.MESSAGE_QUERY; }

        return response;
    }
}
