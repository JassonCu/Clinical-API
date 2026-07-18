using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Prescription.Commands.DeleteCommand
{
    public class DeletePrescriptionHandler : IRequestHandler<DeletePrescriptionCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePrescriptionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<bool>> Handle(DeletePrescriptionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();
            try
            {
                response.Data = await _unitOfWork.Prescription.ExecAsync(StoreProcedures.uspPrescriptionRemove, new { request.PrescriptionId });
                if (response.Data) { response.IsSuccess = true; response.Message = GlobalMessage.MESSAGE_DELETE; }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return response;
        }
    }
}
