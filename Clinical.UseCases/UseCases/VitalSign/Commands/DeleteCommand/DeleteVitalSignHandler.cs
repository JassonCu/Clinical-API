using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.VitalSign.Commands.DeleteCommand
{
    public class DeleteVitalSignHandler : IRequestHandler<DeleteVitalSignCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteVitalSignHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<BaseResponse<bool>> Handle(DeleteVitalSignCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            response.Data = await _unitOfWork.VitalSign.ExecAsync(StoreProcedures.uspVitalSignRemove, request);
            if (response.Data) { response.IsSuccess = true; response.Message = GlobalMessage.MESSAGE_DELETE; }

            return response;
        }
    }
}
