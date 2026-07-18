using AutoMapper;
using Clinical.Application.DTOS.Doctor.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;
using Clinical.UseCases.Commons.Exceptions;

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

        var doctor = await _unitOfWork.Doctor.GetByIdAsync(StoreProcedures.uspDoctorById, request);

        if (doctor is null) throw new NotFoundException(GlobalMessage.MESSAGE_QUERY_EMPTY);

        response.IsSuccess = true;
        response.Data = _mapper.Map<GetDoctorByIdResponseDto>(doctor);
        response.Message = GlobalMessage.MESSAGE_QUERY;

        return response;
    }
}
