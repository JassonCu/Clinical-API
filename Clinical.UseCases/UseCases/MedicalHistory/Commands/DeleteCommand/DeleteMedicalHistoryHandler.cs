using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.MedicalHistory.Commands.DeleteCommand;

public class DeleteMedicalHistoryHandler : IRequestHandler<DeleteMedicalHistoryCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMedicalHistoryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(DeleteMedicalHistoryCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();
        try
        {
            response.Data = await _unitOfWork.MedicalHistory.ExecAsync(StoreProcedures.uspMedicalHistoryRemove, request);
            if (response.Data) { response.IsSuccess = true; response.Message = GlobalMessage.MESSAGE_DELETE; }
        }
        catch (Exception ex) { response.Message = ex.Message; }
        return response;
    }
}
