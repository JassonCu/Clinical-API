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
        public List<CreatePrescriptionDetailItem> Details { get; set; } = [];

        /// <summary>Set true when the prescriber explicitly confirmed despite an allergy warning.</summary>
        public bool AcknowledgeAllergyWarning { get; set; }
    }

    public class CreatePrescriptionDetailItem
    {
        public int MedicineId { get; set; }
        public int Quantity { get; set; }
        public string? Dosage { get; set; }
        public string? Frequency { get; set; }
        public string? Duration { get; set; }
        public string? Instructions { get; set; }
    }
}
