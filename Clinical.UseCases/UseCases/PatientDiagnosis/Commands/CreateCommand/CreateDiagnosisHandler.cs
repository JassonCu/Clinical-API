using AutoMapper;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using Clinical.Utils.HelperExtensions;
using MediatR;
using Entity = Clinical.Domain.Entities;

namespace Clinical.UseCases.UseCases.PatientDiagnosis.Commands.CreateCommand
{
    public class CreateDiagnosisHandler : IRequestHandler<CreateDiagnosisCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateDiagnosisHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<bool>> Handle(CreateDiagnosisCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var entity = _mapper.Map<Entity.PatientDiagnosis>(request);
                entity.State = 1;
                var parameters = entity.GetPropertiesWithValues();
                response.Data = await _unitOfWork.PatientDiagnosis.ExecAsync(StoreProcedures.uspDiagnosisRegister, parameters);
                if (response.Data) { response.IsSuccess = true; response.Message = GlobalMessage.MESSAGE_SAVE; }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return response;
        }
    }
}
