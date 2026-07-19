using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;
using Clinical.UseCases.Commons.Exceptions;

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

            response.Data = await _unitOfWork.Prescription.ExecAsync(StoreProcedures.uspPrescriptionRemove, new { request.PrescriptionId });
            if (!response.Data)
                throw new NotFoundException(GlobalMessage.MESSAGE_QUERY_EMPTY);

            response.IsSuccess = true;
            response.Message = GlobalMessage.MESSAGE_DELETE;

            return response;
        }
    }
}
