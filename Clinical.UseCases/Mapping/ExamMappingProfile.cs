using AutoMapper;
using Clinical.Application.DTOS.Exam.Response;
using Clinical.Domain.Entities;
using Clinical.UseCases.UseCases.Exam.Commands.ChangeStateCommand;
using Clinical.UseCases.UseCases.Exam.Commands.CreateCommand;
using Clinical.UseCases.UseCases.Exam.Commands.UpdateCommand;

namespace Clinical.UseCases.Mapping;

public class ExamMappingProfile : Profile
{
    public ExamMappingProfile()
    {
        CreateMap<Exam, GetExamByIdResponseDto>().ReverseMap();

        CreateMap<CreateExamCommand, Exam>();
        CreateMap<UpdateExamCommand, Exam>();
        CreateMap<ChangeStateExamCommand, Exam>();
    }
}
