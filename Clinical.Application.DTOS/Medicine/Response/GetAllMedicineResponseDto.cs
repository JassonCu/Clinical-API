namespace Clinical.Application.DTOS.Medicine.Response
{
    public class GetAllMedicineResponseDto
    {
        public int MedicineId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? GenericName { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }
        public string? Presentation { get; set; }
        public string? Concentration { get; set; }
        public int? CurrentStock { get; set; }
        public int? MinimumStock { get; set; }
        public decimal? Price { get; set; }
        public bool? RequiresPrescription { get; set; }
        public bool IsLowStock => CurrentStock.HasValue && MinimumStock.HasValue && CurrentStock <= MinimumStock;
        public int State { get; set; }
        public string? StateMedicine => State == 1 ? "ACTIVO" : "INACTIVO";
    }
}
