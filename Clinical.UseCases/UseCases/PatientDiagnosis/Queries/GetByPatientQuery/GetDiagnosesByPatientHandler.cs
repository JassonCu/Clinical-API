using AutoMapper;
using Clinical.Application.DTOS.PatientDiagnosis.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.PatientDiagnosis.Queries.GetByPatientQuery
{
    public class GetDiagnosesByPatientHandler : IRequestHandler<GetDiagnosesByPatientQuery, BaseResponse<IEnumerable<GetAllDiagnosisResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDiagnosesByPatientHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<IEnumerable<GetAllDiagnosisResponseDto>>> Handle(GetDiagnosesByPatientQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<GetAllDiagnosisResponseDto>>();
            try
            {
                var entities = await _unitOfWork.PatientDiagnosis.GetAllAsync(StoreProcedures.uspDiagnosisByPatient, new { PatientId = request.PatientId });
                if (entities is not null) { response.IsSuccess = true; response.Data = _mapper.Map<IEnumerable<GetAllDiagnosisResponseDto>>(entities); response.Message = GlobalMessage.MESSAGE_QUERY; }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return response;
        }
    }
}
