using AutoMapper;
using Clinical.Domain.Entities;
using Clinical.UseCases.UseCases.ExamResult.Commands.ChangeStateCommand;
using Clinical.UseCases.UseCases.ExamResult.Commands.CreateCommand;
using Clinical.UseCases.UseCases.ExamResult.Commands.UpdateCommand;

namespace Clinical.UseCases.Mapping;

public class ExamResultMappingProfile : Profile
{
    public ExamResultMappingProfile()
    {
        CreateMap<CreateExamResultCommand, ExamResult>();
        CreateMap<UpdateExamResultCommand, ExamResult>();
        CreateMap<ChangeStateExamResultCommand, ExamResult>();
    }
}
