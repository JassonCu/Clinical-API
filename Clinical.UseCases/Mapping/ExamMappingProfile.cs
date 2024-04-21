using AutoMapper;
using Clinical.Application.DTOS.Exam.Response;
using Clinical.Domain.Entities;

namespace Clinical.UseCases.Mapping
{
    public class ExamMappingProfile : Profile
    {
        public ExamMappingProfile()
        {
            CreateMap<Exam, GetExamByIdResponseDto>()
                .ReverseMap();
        }
    }
}
