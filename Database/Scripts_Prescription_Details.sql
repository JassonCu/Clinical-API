-- =============================================================================
-- Persistencia de detalles de receta (medicamentos)
-- =============================================================================
-- Habilita guardar los medicamentos de una receta:
--   * uspPrescriptionRegister ahora DEVUELVE el nuevo PrescriptionId (SCOPE_IDENTITY).
--   * uspPrescriptionDetailRegister inserta una línea de detalle.
-- La app inserta cabecera + detalles en una sola transacción.
-- Ejecutar una vez contra la base Clinical.
-- =============================================================================

USE Clinical;
GO

CREATE OR ALTER PROCEDURE uspPrescriptionRegister
    @PatientId INT, @DoctorId INT, @AppointmentId INT,
    @PrescriptionDate DATETIME2, @ValidUntil DATETIME2, @Notes NVARCHAR(MAX), @State TINYINT = 1
AS
BEGIN
    INSERT INTO Prescription (PatientId, DoctorId, AppointmentId, PrescriptionDate, ValidUntil, Notes, State)
    VALUES (@PatientId, @DoctorId, @AppointmentId, @PrescriptionDate, @ValidUntil, @Notes, @State);

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
GO

CREATE OR ALTER PROCEDURE uspPrescriptionDetailRegister
    @PrescriptionId INT, @MedicineId INT, @Quantity INT,
    @Dosage NVARCHAR(100), @Frequency NVARCHAR(100), @Duration NVARCHAR(100), @Instructions NVARCHAR(500)
AS
BEGIN
    INSERT INTO PrescriptionDetail (PrescriptionId, MedicineId, Quantity, Dosage, Frequency, Duration, Instructions, State)
    VALUES (@PrescriptionId, @MedicineId, @Quantity, @Dosage, @Frequency, @Duration, @Instructions, 1);
END
GO
