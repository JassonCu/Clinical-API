using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Medicine.Commands.ChangeStateCommand;

public class ChangeStateMedicineCommand : IRequest<BaseResponse<bool>>
{
    public int MedicineId { get; set; }
    public int State { get; set; }
}