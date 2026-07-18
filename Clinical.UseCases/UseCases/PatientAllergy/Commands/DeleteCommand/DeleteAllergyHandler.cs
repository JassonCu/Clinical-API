using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.PatientAllergy.Commands.DeleteCommand
{
    public class DeleteAllergyHandler : IRequestHandler<DeleteAllergyCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAllergyHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<bool>> Handle(DeleteAllergyCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            response.Data = await _unitOfWork.PatientAllergy.ExecAsync(StoreProcedures.uspAllergyRemove, new { request.AllergyId });
            if (response.Data) { response.IsSuccess = true; response.Message = GlobalMessage.MESSAGE_DELETE; }

            return response;
        }
    }
}
