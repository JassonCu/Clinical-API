using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Doctor.Commands.UpdateCommand;

public class UpdateDoctorCommand : IRequest<BaseResponse<bool>>
{
    public int DoctorId { get; set; }
    public string? DocumentNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Specialty { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? MedicalLicense { get; set; }
}
