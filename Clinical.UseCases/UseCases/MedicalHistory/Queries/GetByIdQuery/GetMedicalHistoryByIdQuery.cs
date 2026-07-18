using Clinical.Application.DTOS.MedicalHistory.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.MedicalHistory.Queries.GetByIdQuery;

public class GetMedicalHistoryByIdQuery : IRequest<BaseResponse<GetMedicalHistoryResponseDto>>
{
    public int MedicalHistoryId { get; set; }
}
