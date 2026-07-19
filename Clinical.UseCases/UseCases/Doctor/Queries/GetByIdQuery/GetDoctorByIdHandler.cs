using AutoMapper;
using Clinical.Application.DTOS.Doctor.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Exceptions;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Doctor.Queries.GetByIdQuery;

public class GetDoctorByIdHandler : IRequestHandler<GetDoctorByIdQuery, GetDoctorByIdResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDoctorByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetDoctorByIdResponseDto> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
    {
        var doctor = await _unitOfWork.Doctor.GetByIdAsync(StoreProcedures.uspDoctorById, request);

        if (doctor is null)
            throw new NotFoundException(GlobalMessage.MESSAGE_QUERY_EMPTY);

        return _mapper.Map<GetDoctorByIdResponseDto>(doctor);
    }
}
