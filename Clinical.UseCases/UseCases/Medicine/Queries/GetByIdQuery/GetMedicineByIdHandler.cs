using AutoMapper;
using Clinical.Application.DTOS.Medicine.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Medicine.Queries.GetByIdQuery;

public class GetMedicineByIdHandler : IRequestHandler<GetMedicineByIdQuery, BaseResponse<GetMedicineByIdResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMedicineByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<GetMedicineByIdResponseDto>> Handle(GetMedicineByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<GetMedicineByIdResponseDto>();
        try
        {
            var entity = await _unitOfWork.Medicine.GetByIdAsync(StoreProcedures.uspMedicineById, request);
            if (entity is null) { response.IsSuccess = false; response.Message = GlobalMessage.MESSAGE_QUERY_EMPTY; return response; }
            response.IsSuccess = true;
            response.Data = _mapper.Map<GetMedicineByIdResponseDto>(entity);
            response.Message = GlobalMessage.MESSAGE_QUERY;
        }
        catch (Exception ex) { response.Message = ex.Message; }
        return response;
    }
}
