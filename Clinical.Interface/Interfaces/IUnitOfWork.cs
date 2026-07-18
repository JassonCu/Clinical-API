using Clinical.Domain.Entities;

namespace Clinical.Interface.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Analysis> Analysis { get; }
        IGenericRepository<Exam> Exam { get; }
        IGenericRepository<Patient> Patient { get; }
        IGenericRepository<Doctor> Doctor { get; }
        IGenericRepository<Appointment> Appointment { get; }
        IGenericRepository<ExamResult> ExamResult { get; }
        IGenericRepository<MedicalHistory> MedicalHistory { get; }
        IGenericRepository<VitalSign> VitalSign { get; }
        IGenericRepository<Medicine> Medicine { get; }
        IGenericRepository<Prescription> Prescription { get; }
        IGenericRepository<PrescriptionDetail> PrescriptionDetail { get; }
        IGenericRepository<PatientAllergy> PatientAllergy { get; }
        IGenericRepository<PatientDiagnosis> PatientDiagnosis { get; }
    }
}
