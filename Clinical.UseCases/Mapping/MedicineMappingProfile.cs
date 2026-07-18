using AutoMapper;
using Clinical.Application.DTOS.Medicine.Response;
using Clinical.Domain.Entities;
using Clinical.UseCases.UseCases.Medicine.Commands.ChangeStateCommand;
using Clinical.UseCases.UseCases.Medicine.Commands.CreateCommand;
using Clinical.UseCases.UseCases.Medicine.Commands.UpdateCommand;

namespace Clinical.UseCases.Mapping;

public class MedicineMappingProfile : Profile
{
    public MedicineMappingProfile()
    {
        CreateMap<CreateMedicineCommand, Medicine>();
        CreateMap<UpdateMedicineCommand, Medicine>();
        CreateMap<ChangeStateMedicineCommand, Medicine>();
        CreateMap<Medicine, GetAllMedicineResponseDto>();
        CreateMap<Medicine, GetMedicineByIdResponseDto>();
    }
}
