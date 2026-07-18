using AutoMapper;
using Clinical.Application.DTOS.Medicine.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Medicine.Queries.GetLowStockQuery;

public class GetLowStockMedicineHandler : IRequestHandler<GetLowStockMedicineQuery, BaseResponse<IEnumerable<GetAllMedicineResponseDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetLowStockMedicineHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<IEnumerable<GetAllMedicineResponseDto>>> Handle(GetLowStockMedicineQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<GetAllMedicineResponseDto>>();
        try
        {
            var entities = await _unitOfWork.Medicine.GetAllAsync(StoreProcedures.uspMedicineLowStock);
            if (entities is not null) { response.IsSuccess = true; response.Data = _mapper.Map<IEnumerable<GetAllMedicineResponseDto>>(entities); response.Message = GlobalMessage.MESSAGE_QUERY; }
        }
        catch (Exception ex) { response.Message = ex.Message; }
        return response;
    }
}
