using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Analysis.Commands.UpdateCommand
{
    public class UpdateAnalysisCommand : IRequest<BaseResponse<bool>>
    {
        public int AnalysisId { get; set; }
        public string? Name { get; set; }
    }
}