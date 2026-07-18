using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.PatientDiagnosis.Commands.DeleteCommand
{
    public class DeleteDiagnosisHandler : IRequestHandler<DeleteDiagnosisCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDiagnosisHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<bool>> Handle(DeleteDiagnosisCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            response.Data = await _unitOfWork.PatientDiagnosis.ExecAsync(StoreProcedures.uspDiagnosisRemove, new { request.DiagnosisId });
            if (response.Data) { response.IsSuccess = true; response.Message = GlobalMessage.MESSAGE_DELETE; }

            return response;
        }
    }
}
