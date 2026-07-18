-- =============================================
-- Clinical API - New Modules DDL & Stored Procedures
-- Modules: Auth, MedicalHistory, VitalSign, Medicine,
--          Prescription, PatientAllergy, PatientDiagnosis
-- =============================================

USE Clinical;
GO

-- =============================================
-- TABLES
-- =============================================

-- Role
CREATE TABLE Role (
    RoleId      INT IDENTITY(1,1) PRIMARY KEY,
    Name        NVARCHAR(50)  NOT NULL UNIQUE,
    Description NVARCHAR(200) NULL,
    State       TINYINT       NOT NULL DEFAULT 1
);
GO

-- User
CREATE TABLE [User] (
    UserId              INT IDENTITY(1,1) PRIMARY KEY,
    Username            NVARCHAR(50)  NOT NULL UNIQUE,
    Email               NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash        NVARCHAR(255) NOT NULL,
    FirstName           NVARCHAR(80)  NOT NULL,
    LastName            NVARCHAR(80)  NOT NULL,
    RoleId              INT           NOT NULL REFERENCES Role(RoleId),
    DoctorId            INT           NULL REFERENCES Doctor(DoctorId),
    RefreshToken        NVARCHAR(255) NULL,
    RefreshTokenExpiry  DATETIME2     NULL,
    LastLoginDate       DATETIME2     NULL,
    State               TINYINT       NOT NULL DEFAULT 1,
    AuditCreateDate     DATETIME2     NOT NULL DEFAULT GETUTCDATE()
);
GO

-- MedicalHistory
CREATE TABLE MedicalHistory (
    MedicalHistoryId    INT IDENTITY(1,1) PRIMARY KEY,
    PatientId           INT           NOT NULL REFERENCES Patient(PatientId),
    BloodType           NVARCHAR(5)   NULL,
    ChronicDiseases     NVARCHAR(MAX) NULL,
    PreviousSurgeries   NVARCHAR(MAX) NULL,
    FamilyHistory       NVARCHAR(MAX) NULL,
    CurrentMedications  NVARCHAR(MAX) NULL,
    Habits              NVARCHAR(MAX) NULL,
    Observations        NVARCHAR(MAX) NULL,
    LastUpdatedDate     DATETIME2     NULL,
    State               TINYINT       NOT NULL DEFAULT 1,
    AuditCreateDate     DATETIME2     NOT NULL DEFAULT GETUTCDATE()
);
GO

-- VitalSign
CREATE TABLE VitalSign (
    VitalSignId              INT IDENTITY(1,1) PRIMARY KEY,
    PatientId                INT           NOT NULL REFERENCES Patient(PatientId),
    AppointmentId            INT           NULL     REFERENCES Appointment(AppointmentId),
    Weight                   DECIMAL(5,2)  NULL,
    Height                   DECIMAL(5,2)  NULL,
    Bmi                      DECIMAL(5,2)  NULL,
    BloodPressureSystolic    INT           NULL,
    BloodPressureDiastolic   INT           NULL,
    HeartRate                INT           NULL,
    Temperature              DECIMAL(4,1)  NULL,
    OxygenSaturation         INT           NULL,
    RespiratoryRate          INT           NULL,
    GlucoseLevel             DECIMAL(6,2)  NULL,
    Notes                    NVARCHAR(500) NULL,
    MeasuredAt               DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
    State                    TINYINT       NOT NULL DEFAULT 1,
    AuditCreateDate          DATETIME2     NOT NULL DEFAULT GETUTCDATE()
);
GO

-- Medicine
CREATE TABLE Medicine (
    MedicineId           INT IDENTITY(1,1) PRIMARY KEY,
    Code                 NVARCHAR(30)   NULL UNIQUE,
    Name                 NVARCHAR(200)  NOT NULL,
    GenericName          NVARCHAR(200)  NULL,
    Brand                NVARCHAR(100)  NULL,
    Category             NVARCHAR(100)  NULL,
    Presentation         NVARCHAR(100)  NULL,
    Concentration        NVARCHAR(100)  NULL,
    Unit                 NVARCHAR(50)   NULL,
    CurrentStock         INT            NOT NULL DEFAULT 0,
    MinimumStock         INT            NOT NULL DEFAULT 10,
    Price                DECIMAL(10,2)  NULL,
    RequiresPrescription BIT            NOT NULL DEFAULT 0,
    StorageConditions    NVARCHAR(300)  NULL,
    ExpirationDate       DATE           NULL,
    State                TINYINT        NOT NULL DEFAULT 1,
    AuditCreateDate      DATETIME2      NOT NULL DEFAULT GETUTCDATE()
);
GO

-- Prescription
CREATE TABLE Prescription (
    PrescriptionId   INT IDENTITY(1,1) PRIMARY KEY,
    PatientId        INT      NOT NULL REFERENCES Patient(PatientId),
    DoctorId         INT      NOT NULL REFERENCES Doctor(DoctorId),
    AppointmentId    INT      NULL     REFERENCES Appointment(AppointmentId),
    PrescriptionDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ValidUntil       DATETIME2 NULL,
    Notes            NVARCHAR(MAX) NULL,
    State            TINYINT  NOT NULL DEFAULT 1, -- 1=ACTIVA 2=DISPENSADA 3=VENCIDA 0=CANCELADA
    AuditCreateDate  DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

-- PrescriptionDetail
CREATE TABLE PrescriptionDetail (
    PrescriptionDetailId INT IDENTITY(1,1) PRIMARY KEY,
    PrescriptionId       INT           NOT NULL REFERENCES Prescription(PrescriptionId),
    MedicineId           INT           NOT NULL REFERENCES Medicine(MedicineId),
    Quantity             INT           NOT NULL DEFAULT 1,
    Dosage               NVARCHAR(100) NULL,
    Frequency            NVARCHAR(100) NULL,
    Duration             NVARCHAR(100) NULL,
    Instructions         NVARCHAR(500) NULL,
    State                TINYINT       NOT NULL DEFAULT 1
);
GO

-- PatientAllergy
CREATE TABLE PatientAllergy (
    AllergyId       INT IDENTITY(1,1) PRIMARY KEY,
    PatientId       INT           NOT NULL REFERENCES Patient(PatientId),
    AllergenType    NVARCHAR(50)  NULL,   -- Medicamento, Alimento, Ambiental, Otro
    AllergenName    NVARCHAR(200) NOT NULL,
    Reaction        NVARCHAR(300) NULL,
    Severity        NVARCHAR(20)  NULL,   -- Leve, Moderada, Grave, Anafilaxia
    Notes           NVARCHAR(500) NULL,
    State           TINYINT       NOT NULL DEFAULT 1,
    AuditCreateDate DATETIME2     NOT NULL DEFAULT GETUTCDATE()
);
GO

-- PatientDiagnosis
CREATE TABLE PatientDiagnosis (
    DiagnosisId     INT IDENTITY(1,1) PRIMARY KEY,
    AppointmentId   INT           NOT NULL REFERENCES Appointment(AppointmentId),
    PatientId       INT           NOT NULL REFERENCES Patient(PatientId),
    IcdCode         NVARCHAR(20)  NULL,
    Description     NVARCHAR(500) NOT NULL,
    DiagnosisType   NVARCHAR(50)  NULL,   -- Principal, Secundario, Presuntivo, Definitivo
    Notes           NVARCHAR(MAX) NULL,
    State           TINYINT       NOT NULL DEFAULT 1,
    AuditCreateDate DATETIME2     NOT NULL DEFAULT GETUTCDATE()
);
GO

-- =============================================
-- SEED DATA
-- =============================================
IF NOT EXISTS (SELECT 1 FROM Role WHERE Name = 'Admin')
BEGIN
    INSERT INTO Role (Name, Description) VALUES
        ('Admin',       'Administrador del sistema'),
        ('Doctor',      'Médico'),
        ('Nurse',       'Enfermero/a'),
        ('Pharmacist',  'Farmacéutico/a'),
        ('Receptionist','Recepcionista');
END
GO


-- =============================================
-- STORED PROCEDURES - AUTH
-- =============================================

CREATE OR ALTER PROCEDURE uspUserByUsername @Username NVARCHAR(50) AS
BEGIN
    SELECT UserId, Username, Email, PasswordHash, FirstName, LastName,
           RoleId, DoctorId, RefreshToken, RefreshTokenExpiry, State
    FROM [User] WHERE Username = @Username AND State = 1;
END
GO

CREATE OR ALTER PROCEDURE uspUserByEmail @Email NVARCHAR(150) AS
BEGIN
    SELECT UserId, Username, Email, PasswordHash, FirstName, LastName,
           RoleId, DoctorId, State
    FROM [User] WHERE Email = @Email AND State = 1;
END
GO

CREATE OR ALTER PROCEDURE uspUserRegister
    @Username NVARCHAR(50), @Email NVARCHAR(150), @PasswordHash NVARCHAR(255),
    @FirstName NVARCHAR(80), @LastName NVARCHAR(80), @RoleId INT
AS
BEGIN
    INSERT INTO [User] (Username, Email, PasswordHash, FirstName, LastName, RoleId)
    VALUES (@Username, @Email, @PasswordHash, @FirstName, @LastName, @RoleId);
END
GO

CREATE OR ALTER PROCEDURE uspUserUpdateRefreshToken
    @UserId INT, @RefreshToken NVARCHAR(255), @RefreshTokenExpiry DATETIME2
AS
BEGIN
    UPDATE [User] SET RefreshToken = @RefreshToken,
        RefreshTokenExpiry = @RefreshTokenExpiry, LastLoginDate = GETUTCDATE()
    WHERE UserId = @UserId;
END
GO

CREATE OR ALTER PROCEDURE uspUserByRefreshToken @RefreshToken NVARCHAR(255) AS
BEGIN
    SELECT UserId, Username, Email, PasswordHash, FirstName, LastName,
           RoleId, RefreshToken, RefreshTokenExpiry, State
    FROM [User] WHERE RefreshToken = @RefreshToken AND State = 1;
END
GO

CREATE OR ALTER PROCEDURE uspRoleById @RoleId INT AS
BEGIN
    SELECT Name FROM Role WHERE RoleId = @RoleId;
END
GO

-- =============================================
-- STORED PROCEDURES - MEDICALHISTORY
-- =============================================

CREATE OR ALTER PROCEDURE uspMedicalHistoryList AS
BEGIN
    SELECT mh.*, CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName
    FROM MedicalHistory mh INNER JOIN Patient p ON mh.PatientId = p.PatientId
    ORDER BY mh.AuditCreateDate DESC;
END
GO

CREATE OR ALTER PROCEDURE uspMedicalHistoryById @MedicalHistoryId INT AS
BEGIN
    SELECT mh.*, CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName
    FROM MedicalHistory mh INNER JOIN Patient p ON mh.PatientId = p.PatientId
    WHERE mh.MedicalHistoryId = @MedicalHistoryId;
END
GO

CREATE OR ALTER PROCEDURE uspMedicalHistoryByPatient @PatientId INT AS
BEGIN
    SELECT mh.*, CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName
    FROM MedicalHistory mh INNER JOIN Patient p ON mh.PatientId = p.PatientId
    WHERE mh.PatientId = @PatientId;
END
GO

CREATE OR ALTER PROCEDURE uspMedicalHistoryRegister
    @PatientId INT, @BloodType NVARCHAR(5), @ChronicDiseases NVARCHAR(MAX),
    @PreviousSurgeries NVARCHAR(MAX), @FamilyHistory NVARCHAR(MAX),
    @CurrentMedications NVARCHAR(MAX), @Habits NVARCHAR(MAX),
    @Observations NVARCHAR(MAX), @State TINYINT
AS
BEGIN
    INSERT INTO MedicalHistory (PatientId, BloodType, ChronicDiseases, PreviousSurgeries,
        FamilyHistory, CurrentMedications, Habits, Observations, LastUpdatedDate, State)
    VALUES (@PatientId, @BloodType, @ChronicDiseases, @PreviousSurgeries,
        @FamilyHistory, @CurrentMedications, @Habits, @Observations, GETUTCDATE(), @State);
END
GO

CREATE OR ALTER PROCEDURE uspMedicalHistoryEdit
    @MedicalHistoryId INT, @BloodType NVARCHAR(5), @ChronicDiseases NVARCHAR(MAX),
    @PreviousSurgeries NVARCHAR(MAX), @FamilyHistory NVARCHAR(MAX),
    @CurrentMedications NVARCHAR(MAX), @Habits NVARCHAR(MAX), @Observations NVARCHAR(MAX)
AS
BEGIN
    UPDATE MedicalHistory SET BloodType = @BloodType, ChronicDiseases = @ChronicDiseases,
        PreviousSurgeries = @PreviousSurgeries, FamilyHistory = @FamilyHistory,
        CurrentMedications = @CurrentMedications, Habits = @Habits,
        Observations = @Observations, LastUpdatedDate = GETUTCDATE()
    WHERE MedicalHistoryId = @MedicalHistoryId;
END
GO

CREATE OR ALTER PROCEDURE uspMedicalHistoryRemove @MedicalHistoryId INT AS
BEGIN
    DELETE FROM MedicalHistory WHERE MedicalHistoryId = @MedicalHistoryId;
END
GO

-- =============================================
-- STORED PROCEDURES - VITALSIGN
-- =============================================

CREATE OR ALTER PROCEDURE uspVitalSignList AS
BEGIN
    SELECT vs.*, CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName
    FROM VitalSign vs INNER JOIN Patient p ON vs.PatientId = p.PatientId
    WHERE vs.State = 1 ORDER BY vs.MeasuredAt DESC;
END
GO

CREATE OR ALTER PROCEDURE uspVitalSignById @VitalSignId INT AS
BEGIN
    SELECT vs.*, CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName
    FROM VitalSign vs INNER JOIN Patient p ON vs.PatientId = p.PatientId
    WHERE vs.VitalSignId = @VitalSignId;
END
GO

CREATE OR ALTER PROCEDURE uspVitalSignByPatient @PatientId INT AS
BEGIN
    SELECT vs.*, CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName
    FROM VitalSign vs INNER JOIN Patient p ON vs.PatientId = p.PatientId
    WHERE vs.PatientId = @PatientId AND vs.State = 1
    ORDER BY vs.MeasuredAt DESC;
END
GO

CREATE OR ALTER PROCEDURE uspVitalSignRegister
    @PatientId INT, @AppointmentId INT, @Weight DECIMAL(5,2), @Height DECIMAL(5,2),
    @Bmi DECIMAL(5,2), @BloodPressureSystolic INT, @BloodPressureDiastolic INT,
    @HeartRate INT, @Temperature DECIMAL(4,1), @OxygenSaturation INT,
    @RespiratoryRate INT, @GlucoseLevel DECIMAL(6,2), @Notes NVARCHAR(500),
    @MeasuredAt DATETIME2, @State TINYINT
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

CREATE OR ALTER PROCEDURE uspVitalSignRemove @VitalSignId INT AS
BEGIN
    UPDATE VitalSign SET State = 0 WHERE VitalSignId = @VitalSignId;
END
GO

-- =============================================
-- STORED PROCEDURES - MEDICINE
-- =============================================

CREATE OR ALTER PROCEDURE uspMedicineList AS
BEGIN
    SELECT * FROM Medicine WHERE State = 1 ORDER BY Name;
END
GO

CREATE OR ALTER PROCEDURE uspMedicineById @MedicineId INT AS
BEGIN
    SELECT * FROM Medicine WHERE MedicineId = @MedicineId;
END
GO

CREATE OR ALTER PROCEDURE uspMedicineLowStock AS
BEGIN
    SELECT * FROM Medicine WHERE CurrentStock <= MinimumStock AND State = 1 ORDER BY CurrentStock;
END
GO

CREATE OR ALTER PROCEDURE uspMedicineRegister
    @Code NVARCHAR(30), @Name NVARCHAR(200), @GenericName NVARCHAR(200),
    @Brand NVARCHAR(100), @Category NVARCHAR(100), @Presentation NVARCHAR(100),
    @Concentration NVARCHAR(100), @Unit NVARCHAR(50), @CurrentStock INT,
    @MinimumStock INT, @Price DECIMAL(10,2), @RequiresPrescription BIT,
    @StorageConditions NVARCHAR(300), @ExpirationDate DATE, @State TINYINT
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

CREATE OR ALTER PROCEDURE uspMedicineEdit
    @MedicineId INT, @Code NVARCHAR(30), @Name NVARCHAR(200), @GenericName NVARCHAR(200),
    @Brand NVARCHAR(100), @Category NVARCHAR(100), @Presentation NVARCHAR(100),
    @Concentration NVARCHAR(100), @Unit NVARCHAR(50), @CurrentStock INT,
    @MinimumStock INT, @Price DECIMAL(10,2), @RequiresPrescription BIT,
    @StorageConditions NVARCHAR(300), @ExpirationDate DATE
AS
BEGIN
    UPDATE Medicine SET Code = @Code, Name = @Name, GenericName = @GenericName,
        Brand = @Brand, Category = @Category, Presentation = @Presentation,
        Concentration = @Concentration, Unit = @Unit, CurrentStock = @CurrentStock,
        MinimumStock = @MinimumStock, Price = @Price,
        RequiresPrescription = @RequiresPrescription,
        StorageConditions = @StorageConditions, ExpirationDate = @ExpirationDate
    WHERE MedicineId = @MedicineId;
END
GO

CREATE OR ALTER PROCEDURE uspMedicineRemove @MedicineId INT AS
BEGIN
    DELETE FROM Medicine WHERE MedicineId = @MedicineId;
END
GO

CREATE OR ALTER PROCEDURE uspMedicineChangeState @MedicineId INT, @State TINYINT AS
BEGIN
    UPDATE Medicine SET State = @State WHERE MedicineId = @MedicineId;
END
GO

-- =============================================
-- STORED PROCEDURES - PRESCRIPTION
-- =============================================

CREATE OR ALTER PROCEDURE uspPrescriptionList AS
BEGIN
    SELECT p.PrescriptionId,
           CONCAT(pa.FirstName, ' ', pa.LastName) AS PatientFullName,
           CONCAT(d.FirstName, ' ', d.LastName)   AS DoctorFullName,
           p.AppointmentId, p.PrescriptionDate, p.ValidUntil, p.State, p.AuditCreateDate
    FROM Prescription p
    INNER JOIN Patient pa ON p.PatientId = pa.PatientId
    INNER JOIN Doctor  d  ON p.DoctorId  = d.DoctorId
    ORDER BY p.PrescriptionDate DESC;
END
GO

CREATE OR ALTER PROCEDURE uspPrescriptionById @PrescriptionId INT AS
BEGIN
    SELECT p.PrescriptionId, p.PatientId,
           CONCAT(pa.FirstName, ' ', pa.LastName) AS PatientFullName,
           p.DoctorId,
           CONCAT(d.FirstName, ' ', d.LastName)   AS DoctorFullName,
           p.AppointmentId, p.PrescriptionDate, p.ValidUntil, p.Notes, p.State,
           pd.PrescriptionDetailId, m.Name AS MedicineName, m.GenericName,
           m.Concentration, pd.Quantity, pd.Dosage, pd.Frequency, pd.Duration, pd.Instructions
    FROM Prescription p
    INNER JOIN Patient pa           ON p.PatientId  = pa.PatientId
    INNER JOIN Doctor  d            ON p.DoctorId   = d.DoctorId
    LEFT  JOIN PrescriptionDetail pd ON p.PrescriptionId = pd.PrescriptionId AND pd.State = 1
    LEFT  JOIN Medicine m            ON pd.MedicineId = m.MedicineId
    WHERE p.PrescriptionId = @PrescriptionId;
END
GO

CREATE OR ALTER PROCEDURE uspPrescriptionByPatient @PatientId INT AS
BEGIN
    SELECT p.PrescriptionId,
           CONCAT(pa.FirstName, ' ', pa.LastName) AS PatientFullName,
           CONCAT(d.FirstName, ' ', d.LastName)   AS DoctorFullName,
           p.AppointmentId, p.PrescriptionDate, p.ValidUntil, p.State, p.AuditCreateDate
    FROM Prescription p
    INNER JOIN Patient pa ON p.PatientId = pa.PatientId
    INNER JOIN Doctor  d  ON p.DoctorId  = d.DoctorId
    WHERE p.PatientId = @PatientId
    ORDER BY p.PrescriptionDate DESC;
END
GO

CREATE OR ALTER PROCEDURE uspPrescriptionByDoctor @DoctorId INT AS
BEGIN
    SELECT p.PrescriptionId,
           CONCAT(pa.FirstName, ' ', pa.LastName) AS PatientFullName,
           CONCAT(d.FirstName, ' ', d.LastName)   AS DoctorFullName,
           p.AppointmentId, p.PrescriptionDate, p.ValidUntil, p.State, p.AuditCreateDate
    FROM Prescription p
    INNER JOIN Patient pa ON p.PatientId = pa.PatientId
    INNER JOIN Doctor  d  ON p.DoctorId  = d.DoctorId
    WHERE p.DoctorId = @DoctorId
    ORDER BY p.PrescriptionDate DESC;
END
GO

CREATE OR ALTER PROCEDURE uspPrescriptionRegister
    @PatientId INT, @DoctorId INT, @AppointmentId INT,
    @PrescriptionDate DATETIME2, @ValidUntil DATETIME2, @Notes NVARCHAR(MAX), @State TINYINT
AS
BEGIN
    INSERT INTO Prescription (PatientId, DoctorId, AppointmentId, PrescriptionDate, ValidUntil, Notes, State)
    VALUES (@PatientId, @DoctorId, @AppointmentId, @PrescriptionDate, @ValidUntil, @Notes, @State);
END
GO

CREATE OR ALTER PROCEDURE uspPrescriptionRemove @PrescriptionId INT AS
BEGIN
    DELETE FROM PrescriptionDetail WHERE PrescriptionId = @PrescriptionId;
    DELETE FROM Prescription WHERE PrescriptionId = @PrescriptionId;
END
GO

CREATE OR ALTER PROCEDURE uspPrescriptionChangeState @PrescriptionId INT, @State TINYINT AS
BEGIN
    UPDATE Prescription SET State = @State WHERE PrescriptionId = @PrescriptionId;
END
GO

-- =============================================
-- STORED PROCEDURES - PATIENT ALLERGY
-- =============================================

CREATE OR ALTER PROCEDURE uspAllergyList AS
BEGIN
    SELECT pa.*, CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName
    FROM PatientAllergy pa INNER JOIN Patient p ON pa.PatientId = p.PatientId
    ORDER BY pa.AuditCreateDate DESC;
END
GO

CREATE OR ALTER PROCEDURE uspAllergyById @AllergyId INT AS
BEGIN
    SELECT pa.*, CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName
    FROM PatientAllergy pa INNER JOIN Patient p ON pa.PatientId = p.PatientId
    WHERE pa.AllergyId = @AllergyId;
END
GO

CREATE OR ALTER PROCEDURE uspAllergyByPatient @PatientId INT AS
BEGIN
    SELECT pa.*, CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName
    FROM PatientAllergy pa INNER JOIN Patient p ON pa.PatientId = p.PatientId
    WHERE pa.PatientId = @PatientId AND pa.State = 1;
END
GO

CREATE OR ALTER PROCEDURE uspAllergyRegister
    @PatientId INT, @AllergenType NVARCHAR(50), @AllergenName NVARCHAR(200),
    @Reaction NVARCHAR(300), @Severity NVARCHAR(20), @Notes NVARCHAR(500), @State TINYINT
AS
BEGIN
    INSERT INTO PatientAllergy (PatientId, AllergenType, AllergenName, Reaction, Severity, Notes, State)
    VALUES (@PatientId, @AllergenType, @AllergenName, @Reaction, @Severity, @Notes, @State);
END
GO

CREATE OR ALTER PROCEDURE uspAllergyEdit
    @AllergyId INT, @AllergenType NVARCHAR(50), @AllergenName NVARCHAR(200),
    @Reaction NVARCHAR(300), @Severity NVARCHAR(20), @Notes NVARCHAR(500)
AS
BEGIN
    UPDATE PatientAllergy SET AllergenType = @AllergenType, AllergenName = @AllergenName,
        Reaction = @Reaction, Severity = @Severity, Notes = @Notes
    WHERE AllergyId = @AllergyId;
END
GO

CREATE OR ALTER PROCEDURE uspAllergyRemove @AllergyId INT AS
BEGIN
    DELETE FROM PatientAllergy WHERE AllergyId = @AllergyId;
END
GO

CREATE OR ALTER PROCEDURE uspAllergyChangeState @AllergyId INT, @State TINYINT AS
BEGIN
    UPDATE PatientAllergy SET State = @State WHERE AllergyId = @AllergyId;
END
GO

-- =============================================
-- STORED PROCEDURES - PATIENT DIAGNOSIS
-- =============================================

CREATE OR ALTER PROCEDURE uspDiagnosisList AS
BEGIN
    SELECT pd.*, CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName
    FROM PatientDiagnosis pd INNER JOIN Patient p ON pd.PatientId = p.PatientId
    ORDER BY pd.AuditCreateDate DESC;
END
GO

CREATE OR ALTER PROCEDURE uspDiagnosisById @DiagnosisId INT AS
BEGIN
    SELECT pd.*, CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName
    FROM PatientDiagnosis pd INNER JOIN Patient p ON pd.PatientId = p.PatientId
    WHERE pd.DiagnosisId = @DiagnosisId;
END
GO

CREATE OR ALTER PROCEDURE uspDiagnosisByPatient @PatientId INT AS
BEGIN
    SELECT pd.*, CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName
    FROM PatientDiagnosis pd INNER JOIN Patient p ON pd.PatientId = p.PatientId
    WHERE pd.PatientId = @PatientId AND pd.State = 1
    ORDER BY pd.AuditCreateDate DESC;
END
GO

CREATE OR ALTER PROCEDURE uspDiagnosisByAppointment @AppointmentId INT AS
BEGIN
    SELECT pd.*, CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName
    FROM PatientDiagnosis pd INNER JOIN Patient p ON pd.PatientId = p.PatientId
    WHERE pd.AppointmentId = @AppointmentId AND pd.State = 1;
END
GO

CREATE OR ALTER PROCEDURE uspDiagnosisRegister
    @AppointmentId INT, @PatientId INT, @IcdCode NVARCHAR(20),
    @Description NVARCHAR(500), @DiagnosisType NVARCHAR(50),
    @Notes NVARCHAR(MAX), @State TINYINT
AS
BEGIN
    INSERT INTO PatientDiagnosis (AppointmentId, PatientId, IcdCode, Description, DiagnosisType, Notes, State)
    VALUES (@AppointmentId, @PatientId, @IcdCode, @Description, @DiagnosisType, @Notes, @State);
END
GO

CREATE OR ALTER PROCEDURE uspDiagnosisRemove @DiagnosisId INT AS
BEGIN
    DELETE FROM PatientDiagnosis WHERE DiagnosisId = @DiagnosisId;
END
GO

CREATE OR ALTER PROCEDURE uspDiagnosisChangeState @DiagnosisId INT, @State TINYINT AS
BEGIN
    UPDATE PatientDiagnosis SET State = @State WHERE DiagnosisId = @DiagnosisId;
END
GO

-- =============================================
-- STORED PROCEDURES - USER MANAGEMENT
-- =============================================

CREATE OR ALTER PROCEDURE uspUserList AS
BEGIN
    SELECT u.UserId, u.Username, u.Email, u.FirstName, u.LastName,
           r.Name AS RoleName, u.RoleId, u.State, u.AuditCreateDate
    FROM [User] u
    INNER JOIN Role r ON u.RoleId = r.RoleId
    ORDER BY u.AuditCreateDate DESC;
END
GO

CREATE OR ALTER PROCEDURE uspUserById @UserId INT AS
BEGIN
    SELECT u.UserId, u.Username, u.Email, u.FirstName, u.LastName,
           r.Name AS RoleName, u.RoleId, u.State, u.AuditCreateDate
    FROM [User] u
    INNER JOIN Role r ON u.RoleId = r.RoleId
    WHERE u.UserId = @UserId;
END
GO

CREATE OR ALTER PROCEDURE uspUserEdit
    @UserId INT, @FirstName NVARCHAR(80), @LastName NVARCHAR(80),
    @Email NVARCHAR(150), @RoleId INT
AS
BEGIN
    UPDATE [User]
    SET FirstName = @FirstName,
        LastName  = @LastName,
        Email     = @Email,
        RoleId    = @RoleId
    WHERE UserId = @UserId;
END
GO

CREATE OR ALTER PROCEDURE uspUserChangeState @UserId INT, @State TINYINT AS
BEGIN
    UPDATE [User] SET State = @State WHERE UserId = @UserId;
END
GO

CREATE OR ALTER PROCEDURE uspRoleList AS
BEGIN
    SELECT RoleId, Name, Description FROM Role ORDER BY RoleId;
END
GO
