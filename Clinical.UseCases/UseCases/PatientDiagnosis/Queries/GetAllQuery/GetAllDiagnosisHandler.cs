using AutoMapper;
using Clinical.Application.DTOS.PatientDiagnosis.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.PatientDiagnosis.Queries.GetAllQuery
{
    public class GetAllDiagnosisHandler : IRequestHandler<GetAllDiagnosisQuery, BaseResponse<IEnumerable<GetAllDiagnosisResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllDiagnosisHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<IEnumerable<GetAllDiagnosisResponseDto>>> Handle(GetAllDiagnosisQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<GetAllDiagnosisResponseDto>>();

            var entities = await _unitOfWork.PatientDiagnosis.GetAllAsync(StoreProcedures.uspDiagnosisList);
            if (entities is not null) { response.IsSuccess = true; response.Data = _mapper.Map<IEnumerable<GetAllDiagnosisResponseDto>>(entities); response.Message = GlobalMessage.MESSAGE_QUERY; }

            return response;
        }
    }
}
