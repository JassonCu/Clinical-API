using Clinical.Application.DTOS.Prescription.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Prescription.Queries.GetByIdQuery
{
    public class GetPrescriptionByIdHandler : IRequestHandler<GetPrescriptionByIdQuery, BaseResponse<GetPrescriptionByIdResponseDto>>
    {
        private readonly IPrescriptionRepository _prescriptionRepository;

        public GetPrescriptionByIdHandler(IPrescriptionRepository prescriptionRepository)
        {
            _prescriptionRepository = prescriptionRepository;
        }

        public async Task<BaseResponse<GetPrescriptionByIdResponseDto>> Handle(GetPrescriptionByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<GetPrescriptionByIdResponseDto>();
            try
            {
                var result = await _prescriptionRepository.GetPrescriptionById(StoreProcedures.uspPrescriptionById, new { request.PrescriptionId });
                if (result is null) { response.IsSuccess = false; response.Message = GlobalMessage.MESSAGE_QUERY_EMPTY; return response; }
                response.IsSuccess = true;
                response.Data = result;
                response.Message = GlobalMessage.MESSAGE_QUERY;
            }
            catch (Exception ex) { response.Message = ex.Message; }
            return response;
        }
    }
}
