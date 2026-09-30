using Clinical.Application.DTOS.Patient.Response;
using Clinical.Interface.Interfaces;
using Clinical.Utils.Constants;

namespace Clinical.UseCases.Services
{
    public class PatientSummaryService : IPatientSummaryService
    {
        private const int RecentLimit = 5;
        private readonly IUnitOfWork _unitOfWork;

        public PatientSummaryService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<PatientSummaryDto?> BuildAsync(int patientId)
        {
            var param = new { PatientId = patientId };

            var patient = await _unitOfWork.Patient.GetByIdAsync(StoreProcedures.uspPatientById, param);
            if (patient is null) return null;

            var allergies = await _unitOfWork.PatientAllergy.GetAllAsync(StoreProcedures.uspAllergyByPatient, param);
            var diagnoses = await _unitOfWork.PatientDiagnosis.GetAllAsync(StoreProcedures.uspDiagnosisByPatient, param);
            var prescriptions = await _unitOfWork.Prescription.GetAllAsync(StoreProcedures.uspPrescriptionByPatient, param);
            var appointments = await _unitOfWork.Appointment.GetAllAsync(StoreProcedures.uspAppointmentByPatient, param);
            var vitals = await _unitOfWork.VitalSign.GetAllAsync(StoreProcedures.uspVitalSignByPatient, param);
            var histories = await _unitOfWork.MedicalHistory.GetAllAsync(StoreProcedures.uspMedicalHistoryByPatient, param);

            var now = DateTime.Now;

            var activeAllergies = allergies
                .Where(a => a.State == 1 && !string.IsNullOrWhiteSpace(a.AllergenName))
                .Select(a => new SummaryAllergyDto
                {
                    AllergenName = a.AllergenName,
                    AllergenType = a.AllergenType,
                    Severity = a.Severity,
                    Reaction = a.Reaction
                })
                .ToList();

            var activeDiagnoses = diagnoses
                .Where(d => d.State == 1)
                .OrderByDescending(d => d.AuditCreateDate)
                .Select(d => new SummaryDiagnosisDto
                {
                    IcdCode = d.IcdCode,
                    Description = d.Description,
                    DiagnosisType = d.DiagnosisType,
                    Date = d.AuditCreateDate
                })
                .ToList();

            var recentPrescriptions = prescriptions
                .OrderByDescending(p => p.PrescriptionDate ?? p.AuditCreateDate)
                .Take(RecentLimit)
                .Select(p => new SummaryPrescriptionDto
                {
                    PrescriptionId = p.PrescriptionId ?? 0,
                    PrescriptionDate = p.PrescriptionDate,
                    ValidUntil = p.ValidUntil,
                    Notes = p.Notes,
                    IsActive = p.State == 1 && (p.ValidUntil is null || p.ValidUntil >= now)
                })
                .ToList();

            var upcomingAppointments = appointments
                .Where(a => a.State == 1 && a.AppointmentDate.HasValue && a.AppointmentDate.Value >= now)
                .OrderBy(a => a.AppointmentDate)
                .Take(RecentLimit)
                .Select(ToAppointmentDto)
                .ToList();

            var lastAppointment = appointments
                .Where(a => a.AppointmentDate.HasValue && a.AppointmentDate.Value < now)
                .OrderByDescending(a => a.AppointmentDate)
                .Select(ToAppointmentDto)
                .FirstOrDefault();

            var latestVital = vitals
                .OrderByDescending(v => v.MeasuredAt ?? v.AuditCreateDate)
                .Select(v => new SummaryVitalSignDto
                {
                    MeasuredAt = v.MeasuredAt ?? v.AuditCreateDate,
                    Weight = v.Weight,
                    Height = v.Height,
                    Bmi = v.Bmi,
                    BloodPressureSystolic = v.BloodPressureSystolic,
                    BloodPressureDiastolic = v.BloodPressureDiastolic,
                    HeartRate = v.HeartRate,
                    Temperature = v.Temperature,
                    OxygenSaturation = v.OxygenSaturation
                })
                .FirstOrDefault();

            var latestHistory = histories
                .OrderByDescending(h => h.LastUpdatedDate ?? h.AuditCreateDate)
                .Select(h => new SummaryMedicalHistoryDto
                {
                    ChronicDiseases = h.ChronicDiseases,
                    PreviousSurgeries = h.PreviousSurgeries,
                    CurrentMedications = h.CurrentMedications,
                    FamilyHistory = h.FamilyHistory,
                    Habits = h.Habits,
                    LastUpdatedDate = h.LastUpdatedDate ?? h.AuditCreateDate
                })
                .FirstOrDefault();

            return new PatientSummaryDto
            {
                Patient = new PatientSummaryHeaderDto
                {
                    PatientId = patient.PatientId ?? patientId,
                    FullName = $"{patient.FirstName} {patient.LastName}".Trim(),
                    DocumentNumber = patient.DocumentNumber,
                    Age = CalculateAge(patient.BirthDate, now),
                    Gender = patient.Gender,
                    BloodType = patient.BloodType,
                    Phone = patient.MobilePhone ?? patient.Phone,
                    InsuranceCompany = patient.InsuranceCompany,
                    EmergencyContactName = patient.EmergencyContactName,
                    EmergencyContactPhone = patient.EmergencyContactPhone
                },
                ActiveAllergies = activeAllergies,
                Diagnoses = activeDiagnoses,
                RecentPrescriptions = recentPrescriptions,
                UpcomingAppointments = upcomingAppointments,
                LastAppointment = lastAppointment,
                LatestVitalSign = latestVital,
                MedicalHistory = latestHistory,
                Counts = new PatientSummaryCountsDto
                {
                    TotalAppointments = appointments.Count(),
                    TotalPrescriptions = prescriptions.Count(),
                    ActiveAllergies = activeAllergies.Count,
                    ActiveDiagnoses = activeDiagnoses.Count
                }
            };
        }

        private static SummaryAppointmentDto ToAppointmentDto(Domain.Entities.Appointment a) => new()
        {
            AppointmentId = a.AppointmentId ?? 0,
            AppointmentDate = a.AppointmentDate,
            Reason = a.Reason,
            State = a.State
        };

        private static int? CalculateAge(DateTime? birthDate, DateTime now)
        {
            if (birthDate is not DateTime dob || dob > now) return null;
            var age = now.Year - dob.Year;
            if (dob.Date > now.AddYears(-age).Date) age--;
            return age;
        }
    }
}
