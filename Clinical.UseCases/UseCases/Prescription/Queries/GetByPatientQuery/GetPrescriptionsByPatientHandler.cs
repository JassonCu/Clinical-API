using Clinical.Application.DTOS.Prescription.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Prescription.Queries.GetByPatientQuery
{
    public class GetPrescriptionsByPatientHandler : IRequestHandler<GetPrescriptionsByPatientQuery, BaseResponse<IEnumerable<GetAllPrescriptionResponseDto>>>
    {
        private readonly IPrescriptionRepository _prescriptionRepository;

        public GetPrescriptionsByPatientHandler(IPrescriptionRepository prescriptionRepository)
        {
            _prescriptionRepository = prescriptionRepository;
        }

        public async Task<BaseResponse<IEnumerable<GetAllPrescriptionResponseDto>>> Handle(GetPrescriptionsByPatientQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<GetAllPrescriptionResponseDto>>();

            var results = await _prescriptionRepository.GetPrescriptionsByPatient(StoreProcedures.uspPrescriptionByPatient, new { request.PatientId });
            if (results is not null) { response.IsSuccess = true; response.Data = results; response.Message = GlobalMessage.MESSAGE_QUERY; }

            return response;
        }
    }
}
