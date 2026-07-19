using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;
using Clinical.UseCases.Commons.Exceptions;

namespace Clinical.UseCases.UseCases.Medicine.Commands.DeleteCommand;

public class DeleteMedicineHandler : IRequestHandler<DeleteMedicineCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    public DeleteMedicineHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(DeleteMedicineCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        response.Data = await _unitOfWork.Medicine.ExecAsync(StoreProcedures.uspMedicineRemove, request);
        if (!response.Data)
            throw new NotFoundException(GlobalMessage.MESSAGE_QUERY_EMPTY);

        response.IsSuccess = true;
        response.Message = GlobalMessage.MESSAGE_DELETE;

        return response;
    }
}
