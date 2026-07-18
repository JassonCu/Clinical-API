using Clinical.Application.DTOS.MedicalHistory.Response;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.MedicalHistory.Queries.GetByPatientQuery;

public class GetMedicalHistoryByPatientQuery : IRequest<BaseResponse<GetMedicalHistoryResponseDto>>
{
    public int PatientId { get; set; }
}
