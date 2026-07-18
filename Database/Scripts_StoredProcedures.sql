-- =============================================
-- CLINICAL API - STORED PROCEDURES SCRIPT
-- =============================================

-- =============================================
-- TABLES
-- =============================================

CREATE TABLE Patient (
    PatientId       INT IDENTITY(1,1) PRIMARY KEY,
    DocumentNumber  NVARCHAR(20)  NOT NULL,
    FirstName       NVARCHAR(100) NOT NULL,
    LastName        NVARCHAR(100) NOT NULL,
    Email           NVARCHAR(150) NULL,
    Phone           NVARCHAR(20)  NULL,
    BirthDate       DATE          NULL,
    Gender          CHAR(1)       NULL,  -- M, F, O
    Address         NVARCHAR(250) NULL,
    State           INT           NOT NULL DEFAULT 1,
    AuditCreateDate DATETIME      NOT NULL DEFAULT GETDATE()
);

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

CREATE TABLE Appointment (
    AppointmentId   INT IDENTITY(1,1) PRIMARY KEY,
    PatientId       INT           NOT NULL REFERENCES Patient(PatientId),
    DoctorId        INT           NOT NULL REFERENCES Doctor(DoctorId),
    AppointmentDate DATETIME      NOT NULL,
    Reason          NVARCHAR(500) NOT NULL,
    Diagnosis       NVARCHAR(1000) NULL,
    Notes           NVARCHAR(1000) NULL,
    State           INT           NOT NULL DEFAULT 1,
    AuditCreateDate DATETIME      NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Analysis (
    AnalysisId      INT IDENTITY(1,1) PRIMARY KEY,
    Name            NVARCHAR(200) NOT NULL,
    State           INT           NOT NULL DEFAULT 1,
    AuditCreateDate DATETIME      NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Exam (
    ExamId          INT IDENTITY(1,1) PRIMARY KEY,
    Name            NVARCHAR(200) NOT NULL,
    AnalysisId      INT           NOT NULL REFERENCES Analysis(AnalysisId),
    State           INT           NOT NULL DEFAULT 1,
    AuditCreateDate DATETIME      NOT NULL DEFAULT GETDATE()
);

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
-- EXAM - MISSING STORED PROCEDURES
-- =============================================

CREATE OR ALTER PROCEDURE uspExamEdit
    @ExamId     INT,
    @Name       NVARCHAR(200),
    @AnalysisId INT
AS
BEGIN
    UPDATE Exam
    SET Name       = @Name,
        AnalysisId = @AnalysisId
    WHERE ExamId = @ExamId;
END;
GO

CREATE OR ALTER PROCEDURE uspExamRemove
    @ExamId INT
AS
BEGIN
    DELETE FROM Exam WHERE ExamId = @ExamId;
END;
GO

CREATE OR ALTER PROCEDURE uspExamChangeState
    @ExamId INT,
    @State  INT
AS
BEGIN
    UPDATE Exam SET State = @State WHERE ExamId = @ExamId;
END;
GO

-- =============================================
-- PATIENT STORED PROCEDURES
-- =============================================

CREATE OR ALTER PROCEDURE uspPatientList
AS
BEGIN
    SELECT PatientId, DocumentNumber, FirstName, LastName, Email, Phone, Gender, State, AuditCreateDate
    FROM Patient
    ORDER BY LastName, FirstName;
END;
GO

CREATE OR ALTER PROCEDURE uspPatientById
    @PatientId INT
AS
BEGIN
    SELECT PatientId, DocumentNumber, FirstName, LastName, Email, Phone, BirthDate, Gender, Address, State, AuditCreateDate
    FROM Patient
    WHERE PatientId = @PatientId;
END;
GO

CREATE OR ALTER PROCEDURE uspPatientRegister
    @DocumentNumber NVARCHAR(20),
    @FirstName      NVARCHAR(100),
    @LastName       NVARCHAR(100),
    @Email          NVARCHAR(150),
    @Phone          NVARCHAR(20),
    @BirthDate      DATE,
    @Gender         CHAR(1),
    @Address        NVARCHAR(250)
AS
BEGIN
    INSERT INTO Patient (DocumentNumber, FirstName, LastName, Email, Phone, BirthDate, Gender, Address)
    VALUES (@DocumentNumber, @FirstName, @LastName, @Email, @Phone, @BirthDate, @Gender, @Address);
END;
GO

CREATE OR ALTER PROCEDURE uspPatientEdit
    @PatientId      INT,
    @DocumentNumber NVARCHAR(20),
    @FirstName      NVARCHAR(100),
    @LastName       NVARCHAR(100),
    @Email          NVARCHAR(150),
    @Phone          NVARCHAR(20),
    @BirthDate      DATE,
    @Gender         CHAR(1),
    @Address        NVARCHAR(250)
AS
BEGIN
    UPDATE Patient
    SET DocumentNumber = @DocumentNumber,
        FirstName      = @FirstName,
        LastName       = @LastName,
        Email          = @Email,
        Phone          = @Phone,
        BirthDate      = @BirthDate,
        Gender         = @Gender,
        Address        = @Address
    WHERE PatientId = @PatientId;
END;
GO

CREATE OR ALTER PROCEDURE uspPatientRemove
    @PatientId INT
AS
BEGIN
    DELETE FROM Patient WHERE PatientId = @PatientId;
END;
GO

CREATE OR ALTER PROCEDURE uspPatientChangeState
    @PatientId INT,
    @State     INT
AS
BEGIN
    UPDATE Patient SET State = @State WHERE PatientId = @PatientId;
END;
GO

-- =============================================
-- DOCTOR STORED PROCEDURES
-- =============================================

CREATE OR ALTER PROCEDURE uspDoctorList
AS
BEGIN
    SELECT DoctorId, DocumentNumber, FirstName, LastName, Specialty, Email, Phone, State, AuditCreateDate
    FROM Doctor
    ORDER BY LastName, FirstName;
END;
GO

CREATE OR ALTER PROCEDURE uspDoctorById
    @DoctorId INT
AS
BEGIN
    SELECT DoctorId, DocumentNumber, FirstName, LastName, Specialty, Email, Phone, MedicalLicense, State, AuditCreateDate
    FROM Doctor
    WHERE DoctorId = @DoctorId;
END;
GO

CREATE OR ALTER PROCEDURE uspDoctorRegister
    @DocumentNumber NVARCHAR(20),
    @FirstName      NVARCHAR(100),
    @LastName       NVARCHAR(100),
    @Specialty      NVARCHAR(100),
    @Email          NVARCHAR(150),
    @Phone          NVARCHAR(20),
    @MedicalLicense NVARCHAR(50)
AS
BEGIN
    INSERT INTO Doctor (DocumentNumber, FirstName, LastName, Specialty, Email, Phone, MedicalLicense)
    VALUES (@DocumentNumber, @FirstName, @LastName, @Specialty, @Email, @Phone, @MedicalLicense);
END;
GO

CREATE OR ALTER PROCEDURE uspDoctorEdit
    @DoctorId       INT,
    @DocumentNumber NVARCHAR(20),
    @FirstName      NVARCHAR(100),
    @LastName       NVARCHAR(100),
    @Specialty      NVARCHAR(100),
    @Email          NVARCHAR(150),
    @Phone          NVARCHAR(20),
    @MedicalLicense NVARCHAR(50)
AS
BEGIN
    UPDATE Doctor
    SET DocumentNumber = @DocumentNumber,
        FirstName      = @FirstName,
        LastName       = @LastName,
        Specialty      = @Specialty,
        Email          = @Email,
        Phone          = @Phone,
        MedicalLicense = @MedicalLicense
    WHERE DoctorId = @DoctorId;
END;
GO

CREATE OR ALTER PROCEDURE uspDoctorRemove
    @DoctorId INT
AS
BEGIN
    DELETE FROM Doctor WHERE DoctorId = @DoctorId;
END;
GO

CREATE OR ALTER PROCEDURE uspDoctorChangeState
    @DoctorId INT,
    @State    INT
AS
BEGIN
    UPDATE Doctor SET State = @State WHERE DoctorId = @DoctorId;
END;
GO

-- =============================================
-- APPOINTMENT STORED PROCEDURES
-- =============================================

CREATE OR ALTER PROCEDURE uspAppointmentList
AS
BEGIN
    SELECT
        a.AppointmentId,
        CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName,
        CONCAT(d.FirstName, ' ', d.LastName) AS DoctorFullName,
        d.Specialty                           AS DoctorSpecialty,
        a.AppointmentDate,
        a.Reason,
        a.State,
        a.AuditCreateDate
    FROM Appointment a
    INNER JOIN Patient p ON a.PatientId = p.PatientId
    INNER JOIN Doctor  d ON a.DoctorId  = d.DoctorId
    ORDER BY a.AppointmentDate DESC;
END;
GO

CREATE OR ALTER PROCEDURE uspAppointmentById
    @AppointmentId INT
AS
BEGIN
    SELECT
        a.AppointmentId,
        a.PatientId,
        CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName,
        a.DoctorId,
        CONCAT(d.FirstName, ' ', d.LastName) AS DoctorFullName,
        d.Specialty                           AS DoctorSpecialty,
        a.AppointmentDate,
        a.Reason,
        a.Diagnosis,
        a.Notes,
        a.State,
        a.AuditCreateDate
    FROM Appointment a
    INNER JOIN Patient p ON a.PatientId = p.PatientId
    INNER JOIN Doctor  d ON a.DoctorId  = d.DoctorId
    WHERE a.AppointmentId = @AppointmentId;
END;
GO

CREATE OR ALTER PROCEDURE uspAppointmentByPatient
    @PatientId INT
AS
BEGIN
    SELECT
        a.AppointmentId,
        CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName,
        CONCAT(d.FirstName, ' ', d.LastName) AS DoctorFullName,
        d.Specialty                           AS DoctorSpecialty,
        a.AppointmentDate,
        a.Reason,
        a.State,
        a.AuditCreateDate
    FROM Appointment a
    INNER JOIN Patient p ON a.PatientId = p.PatientId
    INNER JOIN Doctor  d ON a.DoctorId  = d.DoctorId
    WHERE a.PatientId = @PatientId
    ORDER BY a.AppointmentDate DESC;
END;
GO

CREATE OR ALTER PROCEDURE uspAppointmentByDoctor
    @DoctorId INT
AS
BEGIN
    SELECT
        a.AppointmentId,
        CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName,
        CONCAT(d.FirstName, ' ', d.LastName) AS DoctorFullName,
        d.Specialty                           AS DoctorSpecialty,
        a.AppointmentDate,
        a.Reason,
        a.State,
        a.AuditCreateDate
    FROM Appointment a
    INNER JOIN Patient p ON a.PatientId = p.PatientId
    INNER JOIN Doctor  d ON a.DoctorId  = d.DoctorId
    WHERE a.DoctorId = @DoctorId
    ORDER BY a.AppointmentDate DESC;
END;
GO

CREATE OR ALTER PROCEDURE uspAppointmentRegister
    @PatientId      INT,
    @DoctorId       INT,
    @AppointmentDate DATETIME,
    @Reason         NVARCHAR(500),
    @Notes          NVARCHAR(1000) = NULL
AS
BEGIN
    INSERT INTO Appointment (PatientId, DoctorId, AppointmentDate, Reason, Notes)
    VALUES (@PatientId, @DoctorId, @AppointmentDate, @Reason, @Notes);
END;
GO

CREATE OR ALTER PROCEDURE uspAppointmentEdit
    @AppointmentId  INT,
    @AppointmentDate DATETIME = NULL,
    @Reason         NVARCHAR(500)  = NULL,
    @Diagnosis      NVARCHAR(1000) = NULL,
    @Notes          NVARCHAR(1000) = NULL
AS
BEGIN
    UPDATE Appointment
    SET AppointmentDate = ISNULL(@AppointmentDate, AppointmentDate),
        Reason          = ISNULL(@Reason, Reason),
        Diagnosis       = ISNULL(@Diagnosis, Diagnosis),
        Notes           = ISNULL(@Notes, Notes)
    WHERE AppointmentId = @AppointmentId;
END;
GO

CREATE OR ALTER PROCEDURE uspAppointmentRemove
    @AppointmentId INT
AS
BEGIN
    DELETE FROM Appointment WHERE AppointmentId = @AppointmentId;
END;
GO

CREATE OR ALTER PROCEDURE uspAppointmentChangeState
    @AppointmentId INT,
    @State         INT
AS
BEGIN
    UPDATE Appointment SET State = @State WHERE AppointmentId = @AppointmentId;
END;
GO

-- =============================================
-- EXAM RESULT STORED PROCEDURES
-- =============================================

CREATE OR ALTER PROCEDURE uspExamResultList
AS
BEGIN
    SELECT
        er.ExamResultId,
        CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName,
        e.Name                                AS ExamName,
        an.Name                               AS AnalysisName,
        er.Result,
        er.ResultDate,
        er.State,
        er.AuditCreateDate
    FROM ExamResult er
    INNER JOIN Patient  p  ON er.PatientId = p.PatientId
    INNER JOIN Exam     e  ON er.ExamId    = e.ExamId
    INNER JOIN Analysis an ON e.AnalysisId = an.AnalysisId
    ORDER BY er.ResultDate DESC;
END;
GO

CREATE OR ALTER PROCEDURE uspExamResultById
    @ExamResultId INT
AS
BEGIN
    SELECT
        er.ExamResultId,
        er.PatientId,
        CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName,
        er.ExamId,
        e.Name                                AS ExamName,
        an.Name                               AS AnalysisName,
        er.AppointmentId,
        er.Result,
        er.Observations,
        er.ResultDate,
        er.State,
        er.AuditCreateDate
    FROM ExamResult er
    INNER JOIN Patient  p  ON er.PatientId = p.PatientId
    INNER JOIN Exam     e  ON er.ExamId    = e.ExamId
    INNER JOIN Analysis an ON e.AnalysisId = an.AnalysisId
    WHERE er.ExamResultId = @ExamResultId;
END;
GO

CREATE OR ALTER PROCEDURE uspExamResultByPatient
    @PatientId INT
AS
BEGIN
    SELECT
        er.ExamResultId,
        CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName,
        e.Name                                AS ExamName,
        an.Name                               AS AnalysisName,
        er.Result,
        er.ResultDate,
        er.State,
        er.AuditCreateDate
    FROM ExamResult er
    INNER JOIN Patient  p  ON er.PatientId = p.PatientId
    INNER JOIN Exam     e  ON er.ExamId    = e.ExamId
    INNER JOIN Analysis an ON e.AnalysisId = an.AnalysisId
    WHERE er.PatientId = @PatientId
    ORDER BY er.ResultDate DESC;
END;
GO

CREATE OR ALTER PROCEDURE uspExamResultByAppointment
    @AppointmentId INT
AS
BEGIN
    SELECT
        er.ExamResultId,
        CONCAT(p.FirstName, ' ', p.LastName) AS PatientFullName,
        e.Name                                AS ExamName,
        an.Name                               AS AnalysisName,
        er.Result,
        er.ResultDate,
        er.State,
        er.AuditCreateDate
    FROM ExamResult er
    INNER JOIN Patient  p  ON er.PatientId = p.PatientId
    INNER JOIN Exam     e  ON er.ExamId    = e.ExamId
    INNER JOIN Analysis an ON e.AnalysisId = an.AnalysisId
    WHERE er.AppointmentId = @AppointmentId
    ORDER BY er.ResultDate DESC;
END;
GO

CREATE OR ALTER PROCEDURE uspExamResultRegister
    @PatientId     INT,
    @ExamId        INT,
    @AppointmentId INT = NULL,
    @Result        NVARCHAR(MAX),
    @Observations  NVARCHAR(MAX) = NULL,
    @ResultDate    DATETIME
AS
BEGIN
    INSERT INTO ExamResult (PatientId, ExamId, AppointmentId, Result, Observations, ResultDate)
    VALUES (@PatientId, @ExamId, @AppointmentId, @Result, @Observations, @ResultDate);
END;
GO

CREATE OR ALTER PROCEDURE uspExamResultEdit
    @ExamResultId INT,
    @Result       NVARCHAR(MAX) = NULL,
    @Observations NVARCHAR(MAX) = NULL,
    @ResultDate   DATETIME      = NULL
AS
BEGIN
    UPDATE ExamResult
    SET Result       = ISNULL(@Result, Result),
        Observations = ISNULL(@Observations, Observations),
        ResultDate   = ISNULL(@ResultDate, ResultDate)
    WHERE ExamResultId = @ExamResultId;
END;
GO

CREATE OR ALTER PROCEDURE uspExamResultRemove
    @ExamResultId INT
AS
BEGIN
    DELETE FROM ExamResult WHERE ExamResultId = @ExamResultId;
END;
GO

CREATE OR ALTER PROCEDURE uspExamResultChangeState
    @ExamResultId INT,
    @State        INT
AS
BEGIN
    UPDATE ExamResult SET State = @State WHERE ExamResultId = @ExamResultId;
END;
GO
