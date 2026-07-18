using Clinical.Application.DTOS.Medicine.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Medicine.Queries.GetLowStockQuery;

public class GetLowStockMedicineQuery : IRequest<BaseResponse<IEnumerable<GetAllMedicineResponseDto>>>
{
}
