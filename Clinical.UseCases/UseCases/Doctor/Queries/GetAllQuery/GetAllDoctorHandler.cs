using AutoMapper;
using Clinical.Application.DTOS.Doctor.Response;
using Clinical.Interface.Interfaces;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Doctor.Queries.GetAllQuery;

public class GetAllDoctorHandler : IRequestHandler<GetAllDoctorQuery, IEnumerable<GetAllDoctorResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllDoctorHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetAllDoctorResponseDto>> Handle(GetAllDoctorQuery request, CancellationToken cancellationToken)
    {
        var doctors = await _unitOfWork.Doctor.GetAllAsync(StoreProcedures.uspDoctorList);
        return _mapper.Map<IEnumerable<GetAllDoctorResponseDto>>(doctors);
    }
}
