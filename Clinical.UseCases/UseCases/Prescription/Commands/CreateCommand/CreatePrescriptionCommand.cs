using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.Prescription.Commands.CreateCommand
{
    public class CreatePrescriptionCommand : IRequest<BaseResponse<bool>>
    {
        public int? PatientId { get; set; }
        public int? DoctorId { get; set; }
        public int? AppointmentId { get; set; }
        public DateTime? PrescriptionDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string? Notes { get; set; }
        public int? State { get; set; }
    }
}
