using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Medicine.Commands.UpdateCommand;

public class UpdateMedicineCommand : IRequest<BaseResponse<bool>>
{
    public int MedicineId { get; set; }
    public string? Name { get; set; }
    public string? GenericName { get; set; }
    public string? Brand { get; set; }
    public string? Category { get; set; }
    public string? Presentation { get; set; }
    public string? Concentration { get; set; }
    public string? Unit { get; set; }
    public int? MinimumStock { get; set; }
    public decimal? Price { get; set; }
    public bool? RequiresPrescription { get; set; }
    public string? StorageConditions { get; set; }
    public DateTime? ExpirationDate { get; set; }
}
