using AutoMapper;
using Clinical.Application.DTOS.Patient.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;
using Clinical.UseCases.Commons.Exceptions;

namespace Clinical.UseCases.UseCases.Patient.Queries.GetByIdQuery;

public class GetPatientByIdHandler : IRequestHandler<GetPatientByIdQuery, BaseResponse<GetPatientByIdResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPatientByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetPatientByIdResponseDto>> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<GetPatientByIdResponseDto>();

        var patient = await _unitOfWork.Patient.GetByIdAsync(StoreProcedures.uspPatientById, request);

        if (patient is null) throw new NotFoundException(GlobalMessage.MESSAGE_QUERY_EMPTY);

        response.IsSuccess = true;
        response.Data = _mapper.Map<GetPatientByIdResponseDto>(patient);
        response.Message = GlobalMessage.MESSAGE_QUERY;

        return response;
    }
}
