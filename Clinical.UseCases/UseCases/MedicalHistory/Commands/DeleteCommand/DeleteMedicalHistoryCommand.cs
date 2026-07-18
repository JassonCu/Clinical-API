using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.MedicalHistory.Commands.DeleteCommand;

public class DeleteMedicalHistoryCommand : IRequest<BaseResponse<bool>>
{
    public int MedicalHistoryId { get; set; }
}
