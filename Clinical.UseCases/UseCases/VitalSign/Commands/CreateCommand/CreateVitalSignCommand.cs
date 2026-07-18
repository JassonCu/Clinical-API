using Clinical.UseCases.Commons.Bases;
using MediatR;

namespace Clinical.UseCases.UseCases.VitalSign.Commands.CreateCommand
{
    public class CreateVitalSignCommand : IRequest<BaseResponse<bool>>
    {
        public int PatientId { get; set; }
        public int? AppointmentId { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public int? BloodPressureSystolic { get; set; }
        public int? BloodPressureDiastolic { get; set; }
        public int? HeartRate { get; set; }
        public decimal? Temperature { get; set; }
        public int? OxygenSaturation { get; set; }
        public int? RespiratoryRate { get; set; }
        public decimal? GlucoseLevel { get; set; }
        public string? Notes { get; set; }
        public DateTime MeasuredAt { get; set; }
    }
}
