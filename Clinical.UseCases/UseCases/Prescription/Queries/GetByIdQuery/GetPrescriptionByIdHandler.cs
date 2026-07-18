using Clinical.Application.DTOS.Prescription.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.Utils.Constants;
using MediatR;
using Clinical.UseCases.Commons.Exceptions;

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

            var result = await _prescriptionRepository.GetPrescriptionById(StoreProcedures.uspPrescriptionById, new { request.PrescriptionId });
            if (result is null) throw new NotFoundException(GlobalMessage.MESSAGE_QUERY_EMPTY);
            response.IsSuccess = true;
            response.Data = result;
            response.Message = GlobalMessage.MESSAGE_QUERY;

            return response;
        }
    }
}
