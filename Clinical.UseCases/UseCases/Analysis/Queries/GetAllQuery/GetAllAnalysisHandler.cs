using AutoMapper;
using Clinical.Application.DTOS.Analysis.Response;
using Clinical.Interface.Interfaces;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Analysis.Queries.GetAllQuery
{
    public class GetAllAnalysisHandler : IRequestHandler<GetAllAnalysisQuery, BaseResponse<IEnumerable<GetAnalysisResponseDto>>>
    {
        // private readonly IAnalysisRepository _analysisRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllAnalysisHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<IEnumerable<GetAnalysisResponseDto>>> Handle(GetAllAnalysisQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<GetAnalysisResponseDto>>();

            try
            {
                var analysis = await _unitOfWork.Analysis.GetAllAsync("uspAnalysisList");

                if (analysis is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<IEnumerable<GetAnalysisResponseDto>>(analysis);
                    response.Message = "Consulta Existosa.";
                }

            }

            catch (Exception ex)
            {

                response.Message = ex.Message;
            }
            return response;
        }
    }
}
