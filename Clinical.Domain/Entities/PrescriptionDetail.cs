namespace Clinical.Domain.Entities
{
    public class PrescriptionDetail
    {
        public int? PrescriptionDetailId { get; set; }
        public int? PrescriptionId { get; set; }
        public int? MedicineId { get; set; }
        public int? Quantity { get; set; }
        public string? Dosage { get; set; }
        public string? Frequency { get; set; }
        public string? Duration { get; set; }
        public string? Instructions { get; set; }
        public int? State { get; set; }
    }
}
