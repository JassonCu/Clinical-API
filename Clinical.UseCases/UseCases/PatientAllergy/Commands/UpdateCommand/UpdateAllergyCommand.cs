using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.PatientAllergy.Commands.UpdateCommand
{
    public class UpdateAllergyCommand : IRequest<BaseResponse<bool>>
    {
        public int? AllergyId { get; set; }
        public int? PatientId { get; set; }
        public string? AllergenType { get; set; }
        public string? AllergenName { get; set; }
        public string? Reaction { get; set; }
        public string? Severity { get; set; }
        public string? Notes { get; set; }
        public int? State { get; set; }
    }
}
