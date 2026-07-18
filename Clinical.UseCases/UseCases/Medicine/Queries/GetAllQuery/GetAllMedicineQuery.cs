using Clinical.Application.DTOS.Medicine.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Medicine.Queries.GetAllQuery;

public class GetAllMedicineQuery : IRequest<BaseResponse<IEnumerable<GetAllMedicineResponseDto>>>
{
}
