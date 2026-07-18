namespace Clinical.Utils.Constants
{
    public class StoreProcedures
    {
        #region Analysis
        public const string uspAnalysisChangeState = "uspAnalysisChangeState";
        public const string uspAnalysisRegister = "uspAnalysisRegister";
        public const string uspAnalysisRemove = "uspAnalysisRemove";
        public const string uspAnalysisEdit = "uspAnalysisEdit";
        public const string uspAnalysisList = "uspAnalysisList";
        public const string uspAnalysisById = "uspAnalysisById";
        #endregion

        #region Exam
        public const string uspExamList = "uspExamList";
        public const string uspExamById = "uspExamById";
        public const string uspExamRegister = "uspExamRegister";
        public const string uspExamEdit = "uspExamEdit";
        public const string uspExamRemove = "uspExamRemove";
        public const string uspExamChangeState = "uspExamChangeState";
        #endregion

        #region Patient
        public const string uspPatientList = "uspPatientList";
        public const string uspPatientById = "uspPatientById";
        public const string uspPatientRegister = "uspPatientRegister";
        public const string uspPatientEdit = "uspPatientEdit";
        public const string uspPatientRemove = "uspPatientRemove";
        public const string uspPatientChangeState = "uspPatientChangeState";
        #endregion

        #region Doctor
        public const string uspDoctorList = "uspDoctorList";
        public const string uspDoctorById = "uspDoctorById";
        public const string uspDoctorRegister = "uspDoctorRegister";
        public const string uspDoctorEdit = "uspDoctorEdit";
        public const string uspDoctorRemove = "uspDoctorRemove";
        public const string uspDoctorChangeState = "uspDoctorChangeState";
        #endregion

        #region Appointment
        public const string uspAppointmentList = "uspAppointmentList";
        public const string uspAppointmentById = "uspAppointmentById";
        public const string uspAppointmentByPatient = "uspAppointmentByPatient";
        public const string uspAppointmentByDoctor = "uspAppointmentByDoctor";
        public const string uspAppointmentRegister = "uspAppointmentRegister";
        public const string uspAppointmentEdit = "uspAppointmentEdit";
        public const string uspAppointmentRemove = "uspAppointmentRemove";
        public const string uspAppointmentChangeState = "uspAppointmentChangeState";
        #endregion

        #region ExamResult
        public const string uspExamResultList = "uspExamResultList";
        public const string uspExamResultById = "uspExamResultById";
        public const string uspExamResultByPatient = "uspExamResultByPatient";
        public const string uspExamResultByAppointment = "uspExamResultByAppointment";
        public const string uspExamResultRegister = "uspExamResultRegister";
        public const string uspExamResultEdit = "uspExamResultEdit";
        public const string uspExamResultRemove = "uspExamResultRemove";
        public const string uspExamResultChangeState = "uspExamResultChangeState";
        #endregion

        #region Auth
        public const string uspUserByUsername = "uspUserByUsername";
        public const string uspUserByEmail = "uspUserByEmail";
        public const string uspUserRegister = "uspUserRegister";
        public const string uspUserUpdateRefreshToken = "uspUserUpdateRefreshToken";
        public const string uspUserByRefreshToken = "uspUserByRefreshToken";
        public const string uspRoleById = "uspRoleById";
        #endregion

        #region User Management
        public const string uspUserList = "uspUserList";
        public const string uspUserById = "uspUserById";
        public const string uspUserEdit = "uspUserEdit";
        public const string uspUserChangeState = "uspUserChangeState";
        public const string uspRoleList = "uspRoleList";
        #endregion

        #region MedicalHistory
        public const string uspMedicalHistoryList = "uspMedicalHistoryList";
        public const string uspMedicalHistoryById = "uspMedicalHistoryById";
        public const string uspMedicalHistoryByPatient = "uspMedicalHistoryByPatient";
        public const string uspMedicalHistoryRegister = "uspMedicalHistoryRegister";
        public const string uspMedicalHistoryEdit = "uspMedicalHistoryEdit";
        public const string uspMedicalHistoryRemove = "uspMedicalHistoryRemove";
        #endregion

        #region VitalSign
        public const string uspVitalSignList = "uspVitalSignList";
        public const string uspVitalSignById = "uspVitalSignById";
        public const string uspVitalSignByPatient = "uspVitalSignByPatient";
        public const string uspVitalSignRegister = "uspVitalSignRegister";
        public const string uspVitalSignRemove = "uspVitalSignRemove";
        #endregion

        #region Medicine
        public const string uspMedicineList = "uspMedicineList";
        public const string uspMedicineById = "uspMedicineById";
        public const string uspMedicineLowStock = "uspMedicineLowStock";
        public const string uspMedicineRegister = "uspMedicineRegister";
        public const string uspMedicineEdit = "uspMedicineEdit";
        public const string uspMedicineRemove = "uspMedicineRemove";
        public const string uspMedicineChangeState = "uspMedicineChangeState";
        #endregion

        #region Prescription
        public const string uspPrescriptionList = "uspPrescriptionList";
        public const string uspPrescriptionById = "uspPrescriptionById";
        public const string uspPrescriptionByPatient = "uspPrescriptionByPatient";
        public const string uspPrescriptionByDoctor = "uspPrescriptionByDoctor";
        public const string uspPrescriptionRegister = "uspPrescriptionRegister";
        public const string uspPrescriptionRemove = "uspPrescriptionRemove";
        public const string uspPrescriptionChangeState = "uspPrescriptionChangeState";
        #endregion

        #region PatientAllergy
        public const string uspAllergyList = "uspAllergyList";
        public const string uspAllergyById = "uspAllergyById";
        public const string uspAllergyByPatient = "uspAllergyByPatient";
        public const string uspAllergyRegister = "uspAllergyRegister";
        public const string uspAllergyEdit = "uspAllergyEdit";
        public const string uspAllergyRemove = "uspAllergyRemove";
        public const string uspAllergyChangeState = "uspAllergyChangeState";
        #endregion

        #region PatientDiagnosis
        public const string uspDiagnosisList = "uspDiagnosisList";
        public const string uspDiagnosisById = "uspDiagnosisById";
        public const string uspDiagnosisByPatient = "uspDiagnosisByPatient";
        public const string uspDiagnosisByAppointment = "uspDiagnosisByAppointment";
        public const string uspDiagnosisRegister = "uspDiagnosisRegister";
        public const string uspDiagnosisRemove = "uspDiagnosisRemove";
        public const string uspDiagnosisChangeState = "uspDiagnosisChangeState";
        #endregion
    }
}
