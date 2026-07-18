using Clinical.Application.DTOS.Prescription.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Prescription.Queries.GetAllQuery
{
    public class GetAllPrescriptionHandler : IRequestHandler<GetAllPrescriptionQuery, BaseResponse<IEnumerable<GetAllPrescriptionResponseDto>>>
    {
        private readonly IPrescriptionRepository _prescriptionRepository;

        public GetAllPrescriptionHandler(IPrescriptionRepository prescriptionRepository)
        {
            _prescriptionRepository = prescriptionRepository;
        }

        public async Task<BaseResponse<IEnumerable<GetAllPrescriptionResponseDto>>> Handle(GetAllPrescriptionQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<GetAllPrescriptionResponseDto>>();
            try
            {
                var results = await _prescriptionRepository.GetAllPrescriptions(StoreProcedures.uspPrescriptionList);
                if (results is not null) { response.IsSuccess = true; response.Data = results; response.Message = GlobalMessage.MESSAGE_QUERY; }
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return response;
        }
    }
}
