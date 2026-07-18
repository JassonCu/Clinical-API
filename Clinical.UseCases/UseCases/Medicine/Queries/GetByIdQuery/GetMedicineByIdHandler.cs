using AutoMapper;
using Clinical.Application.DTOS.Medicine.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;
using Clinical.UseCases.Commons.Exceptions;

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

        var entity = await _unitOfWork.Medicine.GetByIdAsync(StoreProcedures.uspMedicineById, request);
        if (entity is null) throw new NotFoundException(GlobalMessage.MESSAGE_QUERY_EMPTY);
        response.IsSuccess = true;
        response.Data = _mapper.Map<GetMedicineByIdResponseDto>(entity);
        response.Message = GlobalMessage.MESSAGE_QUERY;

        return response;
    }
}
