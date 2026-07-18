using AutoMapper;
using Clinical.Application.DTOS.Patient.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

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

        try
        {
            var patient = await _unitOfWork.Patient.GetByIdAsync(StoreProcedures.uspPatientById, request);

            if (patient is null)
            {
                response.IsSuccess = false;
                response.Message = GlobalMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.IsSuccess = true;
            response.Data = _mapper.Map<GetPatientByIdResponseDto>(patient);
            response.Message = GlobalMessage.MESSAGE_QUERY;
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
