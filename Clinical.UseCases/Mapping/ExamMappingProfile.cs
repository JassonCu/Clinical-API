using AutoMapper;
using Clinical.Application.DTOS.Exam.Response;
using Clinical.Domain.Entities;
using Clinical.UseCases.UseCases.Exam.Commands.CreateCommand;

namespace Clinical.UseCases.Mapping
{
    public class ExamMappingProfile : Profile
    {
        public ExamMappingProfile()
        {
            CreateMap<Exam, GetExamByIdResponseDto>()
                .ReverseMap();
                
            CreateMap<CreateExamCommand, Exam>();
        }
    }
}
