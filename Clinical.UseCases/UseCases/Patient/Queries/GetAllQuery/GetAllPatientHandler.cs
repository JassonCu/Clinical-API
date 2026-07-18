using AutoMapper;
using Clinical.Application.DTOS.Patient.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Patient.Queries.GetAllQuery;

public class GetAllPatientHandler : IRequestHandler<GetAllPatientQuery, BaseResponse<IEnumerable<GetAllPatientResponseDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllPatientHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<IEnumerable<GetAllPatientResponseDto>>> Handle(GetAllPatientQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<GetAllPatientResponseDto>>();

        var patients = await _unitOfWork.Patient.GetAllAsync(StoreProcedures.uspPatientList);

        if (patients is not null)
        {
            response.IsSuccess = true;
            response.Data = _mapper.Map<IEnumerable<GetAllPatientResponseDto>>(patients);
            response.Message = GlobalMessage.MESSAGE_QUERY;
        }

        return response;
    }
}
