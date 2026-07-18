using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Patient.Commands.DeleteCommand;

public class DeletePatientHandler : IRequestHandler<DeletePatientCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePatientHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseResponse<bool>> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            response.Data = await _unitOfWork.Patient.ExecAsync(StoreProcedures.uspPatientRemove, request);

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = GlobalMessage.MESSAGE_DELETE;
            }
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
