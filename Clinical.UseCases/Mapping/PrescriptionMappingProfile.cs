using AutoMapper;
using Clinical.Domain.Entities;
using Clinical.UseCases.UseCases.Prescription.Commands.ChangeStateCommand;
using Clinical.UseCases.UseCases.Prescription.Commands.CreateCommand;

namespace Clinical.UseCases.Mapping;

public class PrescriptionMappingProfile : Profile
{
    public PrescriptionMappingProfile()
    {
        CreateMap<CreatePrescriptionCommand, Prescription>();
        CreateMap<ChangeStatePrescriptionCommand, Prescription>();
    }
}
