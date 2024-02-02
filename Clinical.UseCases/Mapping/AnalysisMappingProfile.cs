using AutoMapper;
using Clinical.Application.DTOS.Analysis.Response;
using Clinical.Domain.Entities;
using Clinical.UseCases.UseCases.Analysis.Commands.CreateCommand;
using Clinical.UseCases.UseCases.Analysis.Commands.UpdateCommand;

namespace Clinical.UseCases.Mapping
{
    public class AnalysisMappingProfile : Profile
    {

        public AnalysisMappingProfile()
        {
            CreateMap<Analysis, GetAnalysisResponseDto>()
            .ForMember(x => x.StateAnalysis, x => x.MapFrom(y => y.State == 1 ? "ACTIVO" : "INACTIVO"))
            .ReverseMap();

            CreateMap<Analysis, GetAnalysisByIdResponseDto>()
            .ReverseMap();

            CreateMap<CreateAnalysisCommand, Analysis>();

            CreateMap<UpdateAnalysisCommand, Analysis>();
        }
    }
}
