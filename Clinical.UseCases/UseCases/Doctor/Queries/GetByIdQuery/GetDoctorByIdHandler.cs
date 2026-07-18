using AutoMapper;
using Clinical.Application.DTOS.Doctor.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Doctor.Queries.GetByIdQuery;

public class GetDoctorByIdHandler : IRequestHandler<GetDoctorByIdQuery, BaseResponse<GetDoctorByIdResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDoctorByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetDoctorByIdResponseDto>> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<GetDoctorByIdResponseDto>();

        try
        {
            var doctor = await _unitOfWork.Doctor.GetByIdAsync(StoreProcedures.uspDoctorById, request);

            if (doctor is null)
            {
                response.IsSuccess = false;
                response.Message = GlobalMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.IsSuccess = true;
            response.Data = _mapper.Map<GetDoctorByIdResponseDto>(doctor);
            response.Message = GlobalMessage.MESSAGE_QUERY;
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
