-- =============================================
-- Clinical API - Master Initialization Script
-- Orden: 1) DB  2) Tablas base  3) Módulos  4) Auth
-- Idempotente: seguro de ejecutar varias veces
-- =============================================

-- 1. Crear base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Clinical')
BEGIN
    CREATE DATABASE Clinical;
    PRINT 'Base de datos Clinical creada.';
END
GO

USE Clinical;
GO

-- =============================================
-- TABLAS BASE (Scripts_StoredProcedures)
-- =============================================

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Patient')
CREATE TABLE Patient (
    PatientId       INT IDENTITY(1,1) PRIMARY KEY,
    DocumentNumber  NVARCHAR(20)  NOT NULL,
    FirstName       NVARCHAR(100) NOT NULL,
    LastName        NVARCHAR(100) NOT NULL,
    Email           NVARCHAR(150) NULL,
    Phone           NVARCHAR(20)  NULL,
    BirthDate       DATE          NULL,
    Gender          CHAR(1)       NULL,
    Address         NVARCHAR(250) NULL,
    State           INT           NOT NULL DEFAULT 1,
    AuditCreateDate DATETIME      NOT NULL DEFAULT GETDATE()
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Doctor')
CREATE TABLE Doctor (
    DoctorId        INT IDENTITY(1,1) PRIMARY KEY,
    DocumentNumber  NVARCHAR(20)  NOT NULL,
    FirstName       NVARCHAR(100) NOT NULL,
    LastName        NVARCHAR(100) NOT NULL,
    Specialty       NVARCHAR(100) NOT NULL,
    Email           NVARCHAR(150) NULL,
    Phone           NVARCHAR(20)  NULL,
    MedicalLicense  NVARCHAR(50)  NOT NULL,
    State           INT           NOT NULL DEFAULT 1,
    AuditCreateDate DATETIME      NOT NULL DEFAULT GETDATE()
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Appointment')
CREATE TABLE Appointment (
    AppointmentId   INT IDENTITY(1,1) PRIMARY KEY,
    PatientId       INT            NOT NULL REFERENCES Patient(PatientId),
    DoctorId        INT            NOT NULL REFERENCES Doctor(DoctorId),
    AppointmentDate DATETIME       NOT NULL,
    Reason          NVARCHAR(500)  NOT NULL,
    Diagnosis       NVARCHAR(1000) NULL,
    Notes           NVARCHAR(1000) NULL,
    State           INT            NOT NULL DEFAULT 1,
    AuditCreateDate DATETIME       NOT NULL DEFAULT GETDATE()
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Analysis')
CREATE TABLE Analysis (
    AnalysisId      INT IDENTITY(1,1) PRIMARY KEY,
    Name            NVARCHAR(200) NOT NULL,
    State           INT           NOT NULL DEFAULT 1,
    AuditCreateDate DATETIME      NOT NULL DEFAULT GETDATE()
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Exam')
CREATE TABLE Exam (
    ExamId          INT IDENTITY(1,1) PRIMARY KEY,
    Name            NVARCHAR(200) NOT NULL,
    AnalysisId      INT           NOT NULL REFERENCES Analysis(AnalysisId),
    State           INT           NOT NULL DEFAULT 1,
    AuditCreateDate DATETIME      NOT NULL DEFAULT GETDATE()
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ExamResult')
CREATE TABLE ExamResult (
    ExamResultId    INT IDENTITY(1,1) PRIMARY KEY,
    PatientId       INT           NOT NULL REFERENCES Patient(PatientId),
    ExamId          INT           NOT NULL REFERENCES Exam(ExamId),
    AppointmentId   INT           NULL     REFERENCES Appointment(AppointmentId),
    Result          NVARCHAR(MAX) NOT NULL,
    Observations    NVARCHAR(MAX) NULL,
    ResultDate      DATETIME      NOT NULL,
    State           INT           NOT NULL DEFAULT 1,
    AuditCreateDate DATETIME      NOT NULL DEFAULT GETDATE()
);
GO

-- =============================================
-- TABLAS MÓDULOS (Scripts_NewModules)
-- =============================================

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Role')
CREATE TABLE Role (
    RoleId      INT IDENTITY(1,1) PRIMARY KEY,
    Name        NVARCHAR(50)  NOT NULL UNIQUE,
    Description NVARCHAR(200) NULL,
    State       TINYINT       NOT NULL DEFAULT 1
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'User')
CREATE TABLE [User] (
    UserId              INT IDENTITY(1,1) PRIMARY KEY,
    Username            NVARCHAR(50)  NOT NULL UNIQUE,
    Email               NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash        NVARCHAR(255) NOT NULL,
    FirstName           NVARCHAR(80)  NOT NULL,
    LastName            NVARCHAR(80)  NOT NULL,
    RoleId              INT           NOT NULL REFERENCES Role(RoleId),
    DoctorId            INT           NULL     REFERENCES Doctor(DoctorId),
    RefreshToken        NVARCHAR(255) NULL,
    RefreshTokenExpiry  DATETIME2     NULL,
    LastLoginDate       DATETIME2     NULL,
    MustChangePassword  BIT           NOT NULL DEFAULT 0,
    State               TINYINT       NOT NULL DEFAULT 1,
    AuditCreateDate     DATETIME2     NOT NULL DEFAULT GETUTCDATE()
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'MedicalHistory')
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

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'VitalSign')
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

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Medicine')
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

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Prescription')
CREATE TABLE Prescription (
    PrescriptionId   INT IDENTITY(1,1) PRIMARY KEY,
    PatientId        INT       NOT NULL REFERENCES Patient(PatientId),
    DoctorId         INT       NOT NULL REFERENCES Doctor(DoctorId),
    AppointmentId    INT       NULL     REFERENCES Appointment(AppointmentId),
    PrescriptionDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ValidUntil       DATETIME2 NULL,
    Notes            NVARCHAR(MAX) NULL,
    State            TINYINT   NOT NULL DEFAULT 1,
    AuditCreateDate  DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PrescriptionDetail')
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

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PatientAllergy')
CREATE TABLE PatientAllergy (
    AllergyId       INT IDENTITY(1,1) PRIMARY KEY,
    PatientId       INT           NOT NULL REFERENCES Patient(PatientId),
    AllergenType    NVARCHAR(50)  NULL,
    AllergenName    NVARCHAR(200) NOT NULL,
    Reaction        NVARCHAR(300) NULL,
    Severity        NVARCHAR(20)  NULL,
    Notes           NVARCHAR(500) NULL,
    State           TINYINT       NOT NULL DEFAULT 1,
    AuditCreateDate DATETIME2     NOT NULL DEFAULT GETUTCDATE()
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PatientDiagnosis')
CREATE TABLE PatientDiagnosis (
    DiagnosisId     INT IDENTITY(1,1) PRIMARY KEY,
    AppointmentId   INT           NOT NULL REFERENCES Appointment(AppointmentId),
    PatientId       INT           NOT NULL REFERENCES Patient(PatientId),
    IcdCode         NVARCHAR(20)  NULL,
    Description     NVARCHAR(500) NOT NULL,
    DiagnosisType   NVARCHAR(50)  NULL,
    Notes           NVARCHAR(MAX) NULL,
    State           TINYINT       NOT NULL DEFAULT 1,
    AuditCreateDate DATETIME2     NOT NULL DEFAULT GETUTCDATE()
);
GO

-- =============================================
-- AUTH EXTENSIONS (Scripts_Setup_Auth)
-- =============================================

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PasswordResetToken')
BEGIN
    CREATE TABLE PasswordResetToken (
        TokenId   INT IDENTITY(1,1) PRIMARY KEY,
        UserId    INT          NOT NULL,
        TokenHash NVARCHAR(64) NOT NULL,
        ExpiresAt DATETIME2    NOT NULL,
        CreatedAt DATETIME2    NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_PRT_User FOREIGN KEY (UserId) REFERENCES [User](UserId) ON DELETE CASCADE
    );
    CREATE INDEX IX_PRT_Hash ON PasswordResetToken (TokenHash);
END
GO

-- =============================================
-- SEED DATA
-- =============================================

IF NOT EXISTS (SELECT 1 FROM Role WHERE Name = 'Admin')
BEGIN
    INSERT INTO Role (Name, Description) VALUES
        ('Admin',        'Administrador del sistema'),
        ('Doctor',       'Médico'),
        ('Nurse',        'Enfermero/a'),
        ('Pharmacist',   'Farmacéutico/a'),
        ('Receptionist', 'Recepcionista');
    PRINT 'Roles insertados.';
END
GO

-- =============================================
-- STORED PROCEDURES - BASE
-- =============================================

CREATE OR ALTER PROCEDURE uspPatientList AS
BEGIN
    SELECT PatientId, DocumentNumber, FirstName, LastName, Email, Phone, Gender, State, AuditCreateDate
    FROM Patient ORDER BY LastName, FirstName;
END
GO

CREATE OR ALTER PROCEDURE uspPatientById @PatientId INT AS
BEGIN
    SELECT PatientId, DocumentNumber, FirstName, LastName, Email, Phone, BirthDate, Gender, Address, State, AuditCreateDate
    FROM Patient WHERE PatientId = @PatientId;
END
GO

CREATE OR ALTER PROCEDURE uspPatientRegister
    @DocumentNumber NVARCHAR(20), @FirstName NVARCHAR(100), @LastName NVARCHAR(100),
    @Email NVARCHAR(150), @Phone NVARCHAR(20), @BirthDate DATE, @Gender CHAR(1), @Address NVARCHAR(250)
AS
BEGIN
    INSERT INTO Patient (DocumentNumber, FirstName, LastName, Email, Phone, BirthDate, Gender, Address)
    VALUES (@DocumentNumber, @FirstName, @LastName, @Email, @Phone, @BirthDate, @Gender, @Address);
END
GO

CREATE OR ALTER PROCEDURE uspPatientEdit
    @PatientId INT, @DocumentNumber NVARCHAR(20), @FirstName NVARCHAR(100), @LastName NVARCHAR(100),
    @Email NVARCHAR(150), @Phone NVARCHAR(20), @BirthDate DATE, @Gender CHAR(1), @Address NVARCHAR(250)
AS
BEGIN
    UPDATE Patient SET DocumentNumber = @DocumentNumber, FirstName = @FirstName, LastName = @LastName,
        Email = @Email, Phone = @Phone, BirthDate = @BirthDate, Gender = @Gender, Address = @Address
    WHERE PatientId = @PatientId;
END
GO

CREATE OR ALTER PROCEDURE uspPatientRemove @PatientId INT AS
BEGIN
    DELETE FROM Patient WHERE PatientId = @PatientId;
END
GO

CREATE OR ALTER PROCEDURE uspPatientChangeState @PatientId INT, @State INT AS
BEGIN
    UPDATE Patient SET State = @State WHERE PatientId = @PatientId;
END
GO

CREATE OR ALTER PROCEDURE uspDoctorList AS
BEGIN
    SELECT DoctorId, DocumentNumber, FirstName, LastName, Specialty, Email, Phone, State, AuditCreateDate
    FROM Doctor ORDER BY LastName, FirstName;
END
GO

CREATE OR ALTER PROCEDURE uspDoctorById @DoctorId INT AS
BEGIN
    SELECT DoctorId, DocumentNumber, FirstName, LastName, Specialty, Email, Phone, MedicalLicense, State, AuditCreateDate
    FROM Doctor WHERE DoctorId = @DoctorId;
END
GO

CREATE OR ALTER PROCEDURE uspDoctorRegister
    @DocumentNumber NVARCHAR(20), @FirstName NVARCHAR(100), @LastName NVARCHAR(100),
    @Specialty NVARCHAR(100), @Email NVARCHAR(150), @Phone NVARCHAR(20), @MedicalLicense NVARCHAR(50)
AS
BEGIN
    INSERT INTO Doctor (DocumentNumber, FirstName, LastName, Specialty, Email, Phone, MedicalLicense)
    VALUES (@DocumentNumber, @FirstName, @LastName, @Specialty, @Email, @Phone, @MedicalLicense);
END
GO

CREATE OR ALTER PROCEDURE uspDoctorEdit
    @DoctorId INT, @DocumentNumber NVARCHAR(20), @FirstName NVARCHAR(100), @LastName NVARCHAR(100),
    @Specialty NVARCHAR(100), @Email NVARCHAR(150), @Phone NVARCHAR(20), @MedicalLicense NVARCHAR(50)
AS
BEGIN
    UPDATE Doctor SET DocumentNumber = @DocumentNumber, FirstName = @FirstName, LastName = @LastName,
        Specialty = @Specialty, Email = @Email, Phone = @Phone, MedicalLicense = @MedicalLicense
    WHERE DoctorId = @DoctorId;
END
GO

CREATE OR ALTER PROCEDURE uspDoctorRemove @DoctorId INT AS
BEGIN DELETE FROM Doctor WHERE DoctorId = @DoctorId; END
GO

CREATE OR ALTER PROCEDURE uspDoctorChangeState @DoctorId INT, @State INT AS
BEGIN UPDATE Doctor SET State = @State WHERE DoctorId = @DoctorId; END
GO

CREATE OR ALTER PROCEDURE uspAppointmentList AS
BEGIN
    SELECT a.AppointmentId,
           CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName,
           CONCAT(d.FirstName, ' ', d.LastName) AS DoctorFullName,
           d.Specialty AS DoctorSpecialty,
           a.AppointmentDate, a.Reason, a.State, a.AuditCreateDate
    FROM Appointment a
    INNER JOIN Patient p ON a.PatientId = p.PatientId
    INNER JOIN Doctor  d ON a.DoctorId  = d.DoctorId
    ORDER BY a.AppointmentDate DESC;
END
GO

CREATE OR ALTER PROCEDURE uspAppointmentById @AppointmentId INT AS
BEGIN
    SELECT a.AppointmentId, a.PatientId,
           CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName,
           a.DoctorId,
           CONCAT(d.FirstName, ' ', d.LastName) AS DoctorFullName,
           d.Specialty AS DoctorSpecialty,
           a.AppointmentDate, a.Reason, a.Diagnosis, a.Notes, a.State, a.AuditCreateDate
    FROM Appointment a
    INNER JOIN Patient p ON a.PatientId = p.PatientId
    INNER JOIN Doctor  d ON a.DoctorId  = d.DoctorId
    WHERE a.AppointmentId = @AppointmentId;
END
GO

CREATE OR ALTER PROCEDURE uspAppointmentByPatient @PatientId INT AS
BEGIN
    SELECT a.AppointmentId,
           CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName,
           CONCAT(d.FirstName, ' ', d.LastName) AS DoctorFullName,
           d.Specialty AS DoctorSpecialty,
           a.AppointmentDate, a.Reason, a.State, a.AuditCreateDate
    FROM Appointment a
    INNER JOIN Patient p ON a.PatientId = p.PatientId
    INNER JOIN Doctor  d ON a.DoctorId  = d.DoctorId
    WHERE a.PatientId = @PatientId ORDER BY a.AppointmentDate DESC;
END
GO

CREATE OR ALTER PROCEDURE uspAppointmentByDoctor @DoctorId INT AS
BEGIN
    SELECT a.AppointmentId,
           CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName,
           CONCAT(d.FirstName, ' ', d.LastName) AS DoctorFullName,
           d.Specialty AS DoctorSpecialty,
           a.AppointmentDate, a.Reason, a.State, a.AuditCreateDate
    FROM Appointment a
    INNER JOIN Patient p ON a.PatientId = p.PatientId
    INNER JOIN Doctor  d ON a.DoctorId  = d.DoctorId
    WHERE a.DoctorId = @DoctorId ORDER BY a.AppointmentDate DESC;
END
GO

CREATE OR ALTER PROCEDURE uspAppointmentRegister
    @PatientId INT, @DoctorId INT, @AppointmentDate DATETIME,
    @Reason NVARCHAR(500), @Notes NVARCHAR(1000) = NULL
AS
BEGIN
    INSERT INTO Appointment (PatientId, DoctorId, AppointmentDate, Reason, Notes)
    VALUES (@PatientId, @DoctorId, @AppointmentDate, @Reason, @Notes);
END
GO

CREATE OR ALTER PROCEDURE uspAppointmentEdit
    @AppointmentId INT, @AppointmentDate DATETIME = NULL,
    @Reason NVARCHAR(500) = NULL, @Diagnosis NVARCHAR(1000) = NULL, @Notes NVARCHAR(1000) = NULL
AS
BEGIN
    UPDATE Appointment SET
        AppointmentDate = ISNULL(@AppointmentDate, AppointmentDate),
        Reason          = ISNULL(@Reason, Reason),
        Diagnosis       = ISNULL(@Diagnosis, Diagnosis),
        Notes           = ISNULL(@Notes, Notes)
    WHERE AppointmentId = @AppointmentId;
END
GO

CREATE OR ALTER PROCEDURE uspAppointmentRemove @AppointmentId INT AS
BEGIN DELETE FROM Appointment WHERE AppointmentId = @AppointmentId; END
GO

CREATE OR ALTER PROCEDURE uspAppointmentChangeState @AppointmentId INT, @State INT AS
BEGIN UPDATE Appointment SET State = @State WHERE AppointmentId = @AppointmentId; END
GO

CREATE OR ALTER PROCEDURE uspExamEdit @ExamId INT, @Name NVARCHAR(200), @AnalysisId INT AS
BEGIN UPDATE Exam SET Name = @Name, AnalysisId = @AnalysisId WHERE ExamId = @ExamId; END
GO

CREATE OR ALTER PROCEDURE uspExamRemove @ExamId INT AS
BEGIN DELETE FROM Exam WHERE ExamId = @ExamId; END
GO

CREATE OR ALTER PROCEDURE uspExamChangeState @ExamId INT, @State INT AS
BEGIN UPDATE Exam SET State = @State WHERE ExamId = @ExamId; END
GO

CREATE OR ALTER PROCEDURE uspExamResultList AS
BEGIN
    SELECT er.ExamResultId,
           CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName,
           e.Name AS ExamName, an.Name AS AnalysisName,
           er.Result, er.ResultDate, er.State, er.AuditCreateDate
    FROM ExamResult er
    INNER JOIN Patient  p  ON er.PatientId = p.PatientId
    INNER JOIN Exam     e  ON er.ExamId    = e.ExamId
    INNER JOIN Analysis an ON e.AnalysisId = an.AnalysisId
    ORDER BY er.ResultDate DESC;
END
GO

CREATE OR ALTER PROCEDURE uspExamResultById @ExamResultId INT AS
BEGIN
    SELECT er.ExamResultId, er.PatientId,
           CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName,
           er.ExamId, e.Name AS ExamName, an.Name AS AnalysisName,
           er.AppointmentId, er.Result, er.Observations, er.ResultDate, er.State, er.AuditCreateDate
    FROM ExamResult er
    INNER JOIN Patient  p  ON er.PatientId = p.PatientId
    INNER JOIN Exam     e  ON er.ExamId    = e.ExamId
    INNER JOIN Analysis an ON e.AnalysisId = an.AnalysisId
    WHERE er.ExamResultId = @ExamResultId;
END
GO

CREATE OR ALTER PROCEDURE uspExamResultByPatient @PatientId INT AS
BEGIN
    SELECT er.ExamResultId,
           CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName,
           e.Name AS ExamName, an.Name AS AnalysisName,
           er.Result, er.ResultDate, er.State, er.AuditCreateDate
    FROM ExamResult er
    INNER JOIN Patient  p  ON er.PatientId = p.PatientId
    INNER JOIN Exam     e  ON er.ExamId    = e.ExamId
    INNER JOIN Analysis an ON e.AnalysisId = an.AnalysisId
    WHERE er.PatientId = @PatientId ORDER BY er.ResultDate DESC;
END
GO

CREATE OR ALTER PROCEDURE uspExamResultByAppointment @AppointmentId INT AS
BEGIN
    SELECT er.ExamResultId,
           CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName,
           e.Name AS ExamName, an.Name AS AnalysisName,
           er.Result, er.ResultDate, er.State, er.AuditCreateDate
    FROM ExamResult er
    INNER JOIN Patient  p  ON er.PatientId = p.PatientId
    INNER JOIN Exam     e  ON er.ExamId    = e.ExamId
    INNER JOIN Analysis an ON e.AnalysisId = an.AnalysisId
    WHERE er.AppointmentId = @AppointmentId ORDER BY er.ResultDate DESC;
END
GO

CREATE OR ALTER PROCEDURE uspExamResultRegister
    @PatientId INT, @ExamId INT, @AppointmentId INT = NULL,
    @Result NVARCHAR(MAX), @Observations NVARCHAR(MAX) = NULL, @ResultDate DATETIME
AS
BEGIN
    INSERT INTO ExamResult (PatientId, ExamId, AppointmentId, Result, Observations, ResultDate)
    VALUES (@PatientId, @ExamId, @AppointmentId, @Result, @Observations, @ResultDate);
END
GO

CREATE OR ALTER PROCEDURE uspExamResultEdit
    @ExamResultId INT, @Result NVARCHAR(MAX) = NULL,
    @Observations NVARCHAR(MAX) = NULL, @ResultDate DATETIME = NULL
AS
BEGIN
    UPDATE ExamResult SET
        Result       = ISNULL(@Result, Result),
        Observations = ISNULL(@Observations, Observations),
        ResultDate   = ISNULL(@ResultDate, ResultDate)
    WHERE ExamResultId = @ExamResultId;
END
GO

CREATE OR ALTER PROCEDURE uspExamResultRemove @ExamResultId INT AS
BEGIN DELETE FROM ExamResult WHERE ExamResultId = @ExamResultId; END
GO

CREATE OR ALTER PROCEDURE uspExamResultChangeState @ExamResultId INT, @State INT AS
BEGIN UPDATE ExamResult SET State = @State WHERE ExamResultId = @ExamResultId; END
GO

-- =============================================
-- STORED PROCEDURES - AUTH / USER
-- =============================================

CREATE OR ALTER PROCEDURE uspUserByUsername @Username NVARCHAR(50) AS
BEGIN
    SELECT UserId, Username, Email, PasswordHash, FirstName, LastName,
           RoleId, DoctorId, RefreshToken, RefreshTokenExpiry, State, MustChangePassword
    FROM [User] WHERE Username = @Username AND State = 1;
END
GO

CREATE OR ALTER PROCEDURE uspUserByEmail @Email NVARCHAR(150) AS
BEGIN
    SELECT UserId, Username, Email, PasswordHash, FirstName, LastName, RoleId, DoctorId, State
    FROM [User] WHERE Email = @Email AND State = 1;
END
GO

CREATE OR ALTER PROCEDURE uspUserRegister
    @Username NVARCHAR(50), @Email NVARCHAR(150), @PasswordHash NVARCHAR(255),
    @FirstName NVARCHAR(100), @LastName NVARCHAR(100), @RoleId INT,
    @MustChangePassword BIT = 1
AS
BEGIN
    INSERT INTO [User] (Username, Email, PasswordHash, FirstName, LastName, RoleId, MustChangePassword)
    VALUES (@Username, @Email, @PasswordHash, @FirstName, @LastName, @RoleId, @MustChangePassword);
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
           RoleId, DoctorId, State, MustChangePassword
    FROM [User] WHERE RefreshToken = @RefreshToken AND State = 1;
END
GO

CREATE OR ALTER PROCEDURE uspRoleById @RoleId INT AS
BEGIN SELECT Name FROM Role WHERE RoleId = @RoleId; END
GO

CREATE OR ALTER PROCEDURE uspRoleList AS
BEGIN SELECT RoleId, Name, Description FROM Role ORDER BY RoleId; END
GO

CREATE OR ALTER PROCEDURE uspUserList AS
BEGIN
    SELECT u.UserId, u.Username, u.Email, u.FirstName, u.LastName,
           r.Name AS RoleName, u.RoleId, u.State, u.AuditCreateDate
    FROM [User] u INNER JOIN Role r ON u.RoleId = r.RoleId
    ORDER BY u.AuditCreateDate DESC;
END
GO

CREATE OR ALTER PROCEDURE uspUserById @UserId INT AS
BEGIN
    SELECT u.UserId, u.Username, u.Email, u.FirstName, u.LastName,
           r.Name AS RoleName, u.RoleId, u.State, u.AuditCreateDate
    FROM [User] u INNER JOIN Role r ON u.RoleId = r.RoleId
    WHERE u.UserId = @UserId;
END
GO

CREATE OR ALTER PROCEDURE uspUserEdit
    @UserId INT, @FirstName NVARCHAR(80), @LastName NVARCHAR(80),
    @Email NVARCHAR(150), @RoleId INT
AS
BEGIN
    UPDATE [User] SET FirstName = @FirstName, LastName = @LastName,
        Email = @Email, RoleId = @RoleId WHERE UserId = @UserId;
END
GO

CREATE OR ALTER PROCEDURE uspUserChangeState @UserId INT, @State TINYINT AS
BEGIN UPDATE [User] SET State = @State WHERE UserId = @UserId; END
GO

CREATE OR ALTER PROCEDURE uspUserUpdatePassword @UserId INT, @PasswordHash NVARCHAR(255) AS
BEGIN
    UPDATE [User] SET PasswordHash = @PasswordHash, MustChangePassword = 0 WHERE UserId = @UserId;
END
GO

-- =============================================
-- STORED PROCEDURES - SETUP
-- =============================================

CREATE OR ALTER PROCEDURE uspSetupIsInitialized AS
BEGIN
    SELECT CAST(CASE WHEN COUNT(1) > 0 THEN 1 ELSE 0 END AS BIT) AS IsInitialized FROM [User];
END
GO

CREATE OR ALTER PROCEDURE uspSetupInit
    @Username NVARCHAR(50), @Email NVARCHAR(150), @PasswordHash NVARCHAR(255),
    @FirstName NVARCHAR(100), @LastName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM [User]) BEGIN RAISERROR('Sistema ya inicializado.', 16, 1); RETURN; END
    DECLARE @AdminRoleId INT;
    SELECT @AdminRoleId = RoleId FROM Role WHERE Name = 'Admin';
    IF @AdminRoleId IS NULL BEGIN RAISERROR('Rol Admin no encontrado.', 16, 1); RETURN; END
    INSERT INTO [User] (Username, Email, PasswordHash, FirstName, LastName, RoleId, State, MustChangePassword)
    VALUES (@Username, @Email, @PasswordHash, @FirstName, @LastName, @AdminRoleId, 1, 0);
END
GO

CREATE OR ALTER PROCEDURE uspPasswordResetTokenCreate
    @UserId INT, @TokenHash NVARCHAR(64), @ExpiresAt DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM PasswordResetToken WHERE UserId = @UserId;
    INSERT INTO PasswordResetToken (UserId, TokenHash, ExpiresAt) VALUES (@UserId, @TokenHash, @ExpiresAt);
END
GO

CREATE OR ALTER PROCEDURE uspPasswordResetTokenByHash @TokenHash NVARCHAR(64) AS
BEGIN
    SELECT t.TokenId, t.UserId, t.ExpiresAt, u.Username, u.Email
    FROM PasswordResetToken t
    INNER JOIN [User] u ON t.UserId = u.UserId
    WHERE t.TokenHash = @TokenHash AND t.ExpiresAt > GETUTCDATE();
END
GO

CREATE OR ALTER PROCEDURE uspPasswordResetTokenDelete @TokenId INT AS
BEGIN DELETE FROM PasswordResetToken WHERE TokenId = @TokenId; END
GO

-- =============================================
-- STORED PROCEDURES - MEDICAL HISTORY
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
    @Observations NVARCHAR(MAX), @State TINYINT = 1
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
BEGIN DELETE FROM MedicalHistory WHERE MedicalHistoryId = @MedicalHistoryId; END
GO

-- =============================================
-- STORED PROCEDURES - VITAL SIGN
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
    WHERE vs.PatientId = @PatientId AND vs.State = 1 ORDER BY vs.MeasuredAt DESC;
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

CREATE OR ALTER PROCEDURE uspVitalSignRemove @VitalSignId INT AS
BEGIN UPDATE VitalSign SET State = 0 WHERE VitalSignId = @VitalSignId; END
GO

-- =============================================
-- STORED PROCEDURES - MEDICINE
-- =============================================

CREATE OR ALTER PROCEDURE uspMedicineList AS
BEGIN SELECT * FROM Medicine WHERE State = 1 ORDER BY Name; END
GO

CREATE OR ALTER PROCEDURE uspMedicineById @MedicineId INT AS
BEGIN SELECT * FROM Medicine WHERE MedicineId = @MedicineId; END
GO

CREATE OR ALTER PROCEDURE uspMedicineLowStock AS
BEGIN SELECT * FROM Medicine WHERE CurrentStock <= MinimumStock AND State = 1 ORDER BY CurrentStock; END
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
BEGIN DELETE FROM Medicine WHERE MedicineId = @MedicineId; END
GO

CREATE OR ALTER PROCEDURE uspMedicineChangeState @MedicineId INT, @State TINYINT AS
BEGIN UPDATE Medicine SET State = @State WHERE MedicineId = @MedicineId; END
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
    INNER JOIN Patient pa             ON p.PatientId  = pa.PatientId
    INNER JOIN Doctor  d              ON p.DoctorId   = d.DoctorId
    LEFT  JOIN PrescriptionDetail pd  ON p.PrescriptionId = pd.PrescriptionId AND pd.State = 1
    LEFT  JOIN Medicine m             ON pd.MedicineId = m.MedicineId
    WHERE p.PrescriptionId = @PrescriptionId;
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

CREATE OR ALTER PROCEDURE uspPrescriptionRemove @PrescriptionId INT AS
BEGIN
    DELETE FROM PrescriptionDetail WHERE PrescriptionId = @PrescriptionId;
    DELETE FROM Prescription WHERE PrescriptionId = @PrescriptionId;
END
GO

CREATE OR ALTER PROCEDURE uspPrescriptionChangeState @PrescriptionId INT, @State TINYINT AS
BEGIN UPDATE Prescription SET State = @State WHERE PrescriptionId = @PrescriptionId; END
GO

-- =============================================
-- STORED PROCEDURES - ALLERGY / DIAGNOSIS
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
    @Reaction NVARCHAR(300), @Severity NVARCHAR(20), @Notes NVARCHAR(500), @State TINYINT = 1
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
BEGIN DELETE FROM PatientAllergy WHERE AllergyId = @AllergyId; END
GO

CREATE OR ALTER PROCEDURE uspAllergyChangeState @AllergyId INT, @State TINYINT AS
BEGIN UPDATE PatientAllergy SET State = @State WHERE AllergyId = @AllergyId; END
GO

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
    WHERE pd.PatientId = @PatientId AND pd.State = 1 ORDER BY pd.AuditCreateDate DESC;
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
    @Description NVARCHAR(500), @DiagnosisType NVARCHAR(50), @Notes NVARCHAR(MAX), @State TINYINT = 1
AS
BEGIN
    INSERT INTO PatientDiagnosis (AppointmentId, PatientId, IcdCode, Description, DiagnosisType, Notes, State)
    VALUES (@AppointmentId, @PatientId, @IcdCode, @Description, @DiagnosisType, @Notes, @State);
END
GO

CREATE OR ALTER PROCEDURE uspDiagnosisRemove @DiagnosisId INT AS
BEGIN DELETE FROM PatientDiagnosis WHERE DiagnosisId = @DiagnosisId; END
GO

CREATE OR ALTER PROCEDURE uspDiagnosisChangeState @DiagnosisId INT, @State TINYINT AS
BEGIN UPDATE PatientDiagnosis SET State = @State WHERE DiagnosisId = @DiagnosisId; END
GO

-- =============================================
-- STORED PROCEDURES - ANALYSIS / EXAM
-- =============================================

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

PRINT 'Inicialización completada exitosamente.';
GO
