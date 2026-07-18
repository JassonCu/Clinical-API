using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.PatientDiagnosis.Commands.ChangeStateCommand
{
    public class ChangeStateDiagnosisHandler : IRequestHandler<ChangeStateDiagnosisCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChangeStateDiagnosisHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<bool>> Handle(ChangeStateDiagnosisCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var entity = _mapper.Map<Entity.PatientDiagnosis>(request);
                var parameters = entity.GetPropertiesWithValues();
                response.Data = await _unitOfWork.PatientDiagnosis.ExecAsync(StoreProcedures.uspDiagnosisChangeState, parameters);
                if (response.Data) { response.IsSuccess = true; response.Message = GlobalMessage.MESSAGE_UPDATE; }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return response;
        }
    }
}
