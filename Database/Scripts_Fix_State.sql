-- =============================================
-- FIX: Agregar @State TINYINT = 1 default a los SPs de registro
--      y crear uspAnalysisRegister / uspExamRegister faltantes
-- Ejecutar en la instancia Docker: Clinical
-- =============================================
USE Clinical;
GO

CREATE OR ALTER PROCEDURE uspMedicalHistoryRegister
    @PatientId INT, @BloodType NVARCHAR(5), @ChronicDiseases NVARCHAR(MAX),
    @PreviousSurgeries NVARCHAR(MAX), @FamilyHistory NVARCHAR(MAX),
    @CurrentMedications NVARCHAR(MAX), @Habits NVARCHAR(MAX),
    @Observations NVARCHAR(MAX), @State TINYINT = 1
AS
BEGIN
    INSERT INTO MedicalHistory (PatientId, BloodType, ChronicDiseases, PreviousSurgeries,
        FamilyHistory, CurrentMedications, Habits, Observations, LastUpdatedDate, State)
    VALUES (@PatientId, @BloodType, @ChronicDiseases, @PreviousSurgeries,
        @FamilyHistory, @CurrentMedications, @Habits, @Observations, GETUTCDATE(), @State);
END
GO

CREATE OR ALTER PROCEDURE uspVitalSignRegister
    @PatientId INT, @AppointmentId INT, @Weight DECIMAL(5,2), @Height DECIMAL(5,2),
    @Bmi DECIMAL(5,2), @BloodPressureSystolic INT, @BloodPressureDiastolic INT,
    @HeartRate INT, @Temperature DECIMAL(4,1), @OxygenSaturation INT,
    @RespiratoryRate INT, @GlucoseLevel DECIMAL(6,2), @Notes NVARCHAR(500),
    @MeasuredAt DATETIME2, @State TINYINT = 1
AS
BEGIN
    INSERT INTO VitalSign (PatientId, AppointmentId, Weight, Height, Bmi,
        BloodPressureSystolic, BloodPressureDiastolic, HeartRate, Temperature,
        OxygenSaturation, RespiratoryRate, GlucoseLevel, Notes, MeasuredAt, State)
    VALUES (@PatientId, @AppointmentId, @Weight, @Height, @Bmi,
        @BloodPressureSystolic, @BloodPressureDiastolic, @HeartRate, @Temperature,
        @OxygenSaturation, @RespiratoryRate, @GlucoseLevel, @Notes, @MeasuredAt, @State);
END
GO

CREATE OR ALTER PROCEDURE uspMedicineRegister
    @Code NVARCHAR(30), @Name NVARCHAR(200), @GenericName NVARCHAR(200),
    @Brand NVARCHAR(100), @Category NVARCHAR(100), @Presentation NVARCHAR(100),
    @Concentration NVARCHAR(100), @Unit NVARCHAR(50), @CurrentStock INT,
    @MinimumStock INT, @Price DECIMAL(10,2), @RequiresPrescription BIT,
    @StorageConditions NVARCHAR(300), @ExpirationDate DATE, @State TINYINT = 1
AS
BEGIN
    INSERT INTO Medicine (Code, Name, GenericName, Brand, Category, Presentation,
        Concentration, Unit, CurrentStock, MinimumStock, Price, RequiresPrescription,
        StorageConditions, ExpirationDate, State)
    VALUES (@Code, @Name, @GenericName, @Brand, @Category, @Presentation,
        @Concentration, @Unit, @CurrentStock, @MinimumStock, @Price, @RequiresPrescription,
        @StorageConditions, @ExpirationDate, @State);
END
GO

CREATE OR ALTER PROCEDURE uspPrescriptionRegister
    @PatientId INT, @DoctorId INT, @AppointmentId INT,
    @PrescriptionDate DATETIME2, @ValidUntil DATETIME2, @Notes NVARCHAR(MAX), @State TINYINT = 1
AS
BEGIN
    INSERT INTO Prescription (PatientId, DoctorId, AppointmentId, PrescriptionDate, ValidUntil, Notes, State)
    VALUES (@PatientId, @DoctorId, @AppointmentId, @PrescriptionDate, @ValidUntil, @Notes, @State);
END
GO

CREATE OR ALTER PROCEDURE uspAllergyRegister
    @PatientId INT, @AllergenType NVARCHAR(50), @AllergenName NVARCHAR(200),
    @Reaction NVARCHAR(300), @Severity NVARCHAR(20), @Notes NVARCHAR(500), @State TINYINT = 1
AS
BEGIN
    INSERT INTO PatientAllergy (PatientId, AllergenType, AllergenName, Reaction, Severity, Notes, State)
    VALUES (@PatientId, @AllergenType, @AllergenName, @Reaction, @Severity, @Notes, @State);
END
GO

CREATE OR ALTER PROCEDURE uspDiagnosisRegister
    @AppointmentId INT, @PatientId INT, @IcdCode NVARCHAR(20),
    @Description NVARCHAR(500), @DiagnosisType NVARCHAR(50), @Notes NVARCHAR(MAX), @State TINYINT = 1
AS
BEGIN
    INSERT INTO PatientDiagnosis (AppointmentId, PatientId, IcdCode, Description, DiagnosisType, Notes, State)
    VALUES (@AppointmentId, @PatientId, @IcdCode, @Description, @DiagnosisType, @Notes, @State);
END
GO

-- SPs faltantes para Analysis y Exam

CREATE OR ALTER PROCEDURE uspAnalysisList AS
BEGIN SELECT * FROM Analysis WHERE State = 1 ORDER BY Name; END
GO

CREATE OR ALTER PROCEDURE uspAnalysisById @AnalysisId INT AS
BEGIN SELECT * FROM Analysis WHERE AnalysisId = @AnalysisId; END
GO

CREATE OR ALTER PROCEDURE uspAnalysisRegister @Name NVARCHAR(200), @State TINYINT = 1 AS
BEGIN INSERT INTO Analysis (Name, State) VALUES (@Name, @State); END
GO

CREATE OR ALTER PROCEDURE uspAnalysisEdit @AnalysisId INT, @Name NVARCHAR(200) AS
BEGIN UPDATE Analysis SET Name = @Name WHERE AnalysisId = @AnalysisId; END
GO

CREATE OR ALTER PROCEDURE uspAnalysisRemove @AnalysisId INT AS
BEGIN DELETE FROM Analysis WHERE AnalysisId = @AnalysisId; END
GO

CREATE OR ALTER PROCEDURE uspAnalysisChangeState @AnalysisId INT, @State INT AS
BEGIN UPDATE Analysis SET State = @State WHERE AnalysisId = @AnalysisId; END
GO

CREATE OR ALTER PROCEDURE uspExamList AS
BEGIN
    SELECT e.ExamId, e.Name, e.AnalysisId, a.Name AS AnalysisName, e.State, e.AuditCreateDate
    FROM Exam e INNER JOIN Analysis a ON e.AnalysisId = a.AnalysisId
    WHERE e.State = 1 ORDER BY a.Name, e.Name;
END
GO

CREATE OR ALTER PROCEDURE uspExamById @ExamId INT AS
BEGIN
    SELECT e.ExamId, e.Name, e.AnalysisId, a.Name AS AnalysisName, e.State, e.AuditCreateDate
    FROM Exam e INNER JOIN Analysis a ON e.AnalysisId = a.AnalysisId
    WHERE e.ExamId = @ExamId;
END
GO

CREATE OR ALTER PROCEDURE uspExamByAnalysis @AnalysisId INT AS
BEGIN
    SELECT e.ExamId, e.Name, e.AnalysisId, a.Name AS AnalysisName, e.State, e.AuditCreateDate
    FROM Exam e INNER JOIN Analysis a ON e.AnalysisId = a.AnalysisId
    WHERE e.AnalysisId = @AnalysisId AND e.State = 1 ORDER BY e.Name;
END
GO

CREATE OR ALTER PROCEDURE uspExamRegister @Name NVARCHAR(200), @AnalysisId INT, @State TINYINT = 1 AS
BEGIN INSERT INTO Exam (Name, AnalysisId, State) VALUES (@Name, @AnalysisId, @State); END
GO

PRINT 'Fix aplicado correctamente.';
GO
