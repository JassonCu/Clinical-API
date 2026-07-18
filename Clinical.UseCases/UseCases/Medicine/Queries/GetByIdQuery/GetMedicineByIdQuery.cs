using Clinical.Application.DTOS.Medicine.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Medicine.Queries.GetByIdQuery;

public class GetMedicineByIdQuery : IRequest<BaseResponse<GetMedicineByIdResponseDto>>
{
    public int MedicineId { get; set; }
}
