using Clinical.Application.DTOS.Prescription.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Prescription.Queries.GetByDoctorQuery
{
    public class GetPrescriptionsByDoctorHandler : IRequestHandler<GetPrescriptionsByDoctorQuery, BaseResponse<IEnumerable<GetAllPrescriptionResponseDto>>>
    {
        private readonly IPrescriptionRepository _prescriptionRepository;

        public GetPrescriptionsByDoctorHandler(IPrescriptionRepository prescriptionRepository)
        {
            _prescriptionRepository = prescriptionRepository;
        }

        public async Task<BaseResponse<IEnumerable<GetAllPrescriptionResponseDto>>> Handle(GetPrescriptionsByDoctorQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<GetAllPrescriptionResponseDto>>();

            var results = await _prescriptionRepository.GetPrescriptionsByDoctor(StoreProcedures.uspPrescriptionByDoctor, new { request.DoctorId });
            if (results is not null) { response.IsSuccess = true; response.Data = results; response.Message = GlobalMessage.MESSAGE_QUERY; }

            return response;
        }
    }
}
