using Clinical.Application.DTOS.Patient.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using Clinical.UseCases.Commons.Exceptions;
using Clinical.Utils.Constants;
using MediatR;

namespace Clinical.UseCases.UseCases.Patient.Queries.GetSummaryQuery
{
    public class GetPatientSummaryHandler : IRequestHandler<GetPatientSummaryQuery, BaseResponse<PatientSummaryDto>>
    {
        private readonly IPatientSummaryService _summaryService;

        public GetPatientSummaryHandler(IPatientSummaryService summaryService) => _summaryService = summaryService;

        public async Task<BaseResponse<PatientSummaryDto>> Handle(GetPatientSummaryQuery request, CancellationToken cancellationToken)
        {
            var summary = await _summaryService.BuildAsync(request.PatientId);

            if (summary is null) throw new NotFoundException(GlobalMessage.MESSAGE_QUERY_EMPTY);

            return new BaseResponse<PatientSummaryDto>
            {
                IsSuccess = true,
                Data = summary,
                Message = GlobalMessage.MESSAGE_QUERY
            };
        }
    }
}
