using System.Net;
using AutoMapper;
using Clinical.Application.DTOS.Analysis.Response;
using Clinical.Interface;
using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Analysis.Queries.GetByIdQuery
{
    public class GetAnalysisByIdHandler : IRequestHandler<GetAnalysisByIdQuery, BaseResponse<GetAnalysisByIdResponseDto>>
    {
        private readonly IAnalysisRepository _analysisRepository;
        private readonly IMapper _mapper;

        public GetAnalysisByIdHandler(IAnalysisRepository analysisRepository, IMapper mapper)
        {
            _analysisRepository = analysisRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetAnalysisByIdResponseDto>> Handle(GetAnalysisByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<GetAnalysisByIdResponseDto>();

            try
            {
                var analysis = await _analysisRepository.AnalysisById(request.AnalysisId);

                if (analysis is null)
                {
                    response.IsSuccess = false;
                    response.Message = "No se encontraron registros.";
                }
                
                response.IsSuccess = true;
                response.Data = _mapper.Map<GetAnalysisByIdResponseDto>(analysis);
                response.Message = "Consulta Exitosa";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
