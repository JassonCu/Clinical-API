using Clinical.Domain.Entities;
using Clinical.Interface.Interfaces;

namespace Clinical.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IGenericRepository<Analysis> Analysis { get; }
        public IGenericRepository<Exam> Exam { get; }
        public IGenericRepository<Patient> Patient { get; }
        public IGenericRepository<Doctor> Doctor { get; }
        public IGenericRepository<Appointment> Appointment { get; }
        public IGenericRepository<ExamResult> ExamResult { get; }
        public IGenericRepository<MedicalHistory> MedicalHistory { get; }
        public IGenericRepository<VitalSign> VitalSign { get; }
        public IGenericRepository<Medicine> Medicine { get; }
        public IGenericRepository<Prescription> Prescription { get; }
        public IGenericRepository<PrescriptionDetail> PrescriptionDetail { get; }
        public IGenericRepository<PatientAllergy> PatientAllergy { get; }
        public IGenericRepository<PatientDiagnosis> PatientDiagnosis { get; }

        public UnitOfWork(
            IGenericRepository<Analysis> analysis,
            IGenericRepository<Exam> exam,
            IGenericRepository<Patient> patient,
            IGenericRepository<Doctor> doctor,
            IGenericRepository<Appointment> appointment,
            IGenericRepository<ExamResult> examResult,
            IGenericRepository<MedicalHistory> medicalHistory,
            IGenericRepository<VitalSign> vitalSign,
            IGenericRepository<Medicine> medicine,
            IGenericRepository<Prescription> prescription,
            IGenericRepository<PrescriptionDetail> prescriptionDetail,
            IGenericRepository<PatientAllergy> patientAllergy,
            IGenericRepository<PatientDiagnosis> patientDiagnosis)
        {
            Analysis = analysis;
            Exam = exam;
            Patient = patient;
            Doctor = doctor;
            Appointment = appointment;
            ExamResult = examResult;
            MedicalHistory = medicalHistory;
            VitalSign = vitalSign;
            Medicine = medicine;
            Prescription = prescription;
            PrescriptionDetail = prescriptionDetail;
            PatientAllergy = patientAllergy;
            PatientDiagnosis = patientDiagnosis;
        }

        public void Dispose() => GC.SuppressFinalize(this);
    }
}
