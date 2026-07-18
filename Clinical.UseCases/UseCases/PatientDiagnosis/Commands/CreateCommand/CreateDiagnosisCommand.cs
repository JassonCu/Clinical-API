using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.PatientDiagnosis.Commands.CreateCommand
{
    public class CreateDiagnosisCommand : IRequest<BaseResponse<bool>>
    {
        public int? AppointmentId { get; set; }
        public int? PatientId { get; set; }
        public string? IcdCode { get; set; }
        public string? Description { get; set; }
        public string? DiagnosisType { get; set; }
        public string? Notes { get; set; }
        public int? State { get; set; }
    }
}
