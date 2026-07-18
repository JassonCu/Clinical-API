using AutoMapper;
using Clinical.Application.DTOS.Medicine.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Medicine.Queries.GetAllQuery;

public class GetAllMedicineHandler : IRequestHandler<GetAllMedicineQuery, BaseResponse<IEnumerable<GetAllMedicineResponseDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllMedicineHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<IEnumerable<GetAllMedicineResponseDto>>> Handle(GetAllMedicineQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<GetAllMedicineResponseDto>>();

        var entities = await _unitOfWork.Medicine.GetAllAsync(StoreProcedures.uspMedicineList);
        if (entities is not null) { response.IsSuccess = true; response.Data = _mapper.Map<IEnumerable<GetAllMedicineResponseDto>>(entities); response.Message = GlobalMessage.MESSAGE_QUERY; }

        return response;
    }
}
