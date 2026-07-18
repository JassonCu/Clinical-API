using AutoMapper;
using Clinical.Application.DTOS.Doctor.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Doctor.Queries.GetAllQuery;

public class GetAllDoctorHandler : IRequestHandler<GetAllDoctorQuery, BaseResponse<IEnumerable<GetAllDoctorResponseDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllDoctorHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<IEnumerable<GetAllDoctorResponseDto>>> Handle(GetAllDoctorQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<GetAllDoctorResponseDto>>();

        try
        {
            var doctors = await _unitOfWork.Doctor.GetAllAsync(StoreProcedures.uspDoctorList);

            if (doctors is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<IEnumerable<GetAllDoctorResponseDto>>(doctors);
                response.Message = GlobalMessage.MESSAGE_QUERY;
            }
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
