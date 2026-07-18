using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Medicine.Commands.DeleteCommand;

public class DeleteMedicineCommand : IRequest<BaseResponse<bool>>
{
    public int MedicineId { get; set; }
}
