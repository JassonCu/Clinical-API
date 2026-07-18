using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.MedicalHistory.Commands.UpdateCommand;

public class UpdateMedicalHistoryCommand : IRequest<BaseResponse<bool>>
{
    public int MedicalHistoryId { get; set; }
    public string? BloodType { get; set; }
    public string? ChronicDiseases { get; set; }
    public string? PreviousSurgeries { get; set; }
    public string? FamilyHistory { get; set; }
    public string? CurrentMedications { get; set; }
    public string? Habits { get; set; }
    public string? Observations { get; set; }
}
