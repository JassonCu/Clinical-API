using AutoMapper;
using Clinical.Application.DTOS.PatientAllergy.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.PatientAllergy.Queries.GetByPatientQuery
{
    public class GetAllergiesByPatientHandler : IRequestHandler<GetAllergiesByPatientQuery, BaseResponse<IEnumerable<GetAllAllergyResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllergiesByPatientHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<IEnumerable<GetAllAllergyResponseDto>>> Handle(GetAllergiesByPatientQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<GetAllAllergyResponseDto>>();

            var entities = await _unitOfWork.PatientAllergy.GetAllAsync(StoreProcedures.uspAllergyByPatient, new { PatientId = request.PatientId });
            if (entities is not null) { response.IsSuccess = true; response.Data = _mapper.Map<IEnumerable<GetAllAllergyResponseDto>>(entities); response.Message = GlobalMessage.MESSAGE_QUERY; }

            return response;
        }
    }
}
