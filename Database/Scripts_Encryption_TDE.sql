-- =============================================================================
-- Cifrado en reposo (Encryption at Rest) — Transparent Data Encryption (TDE)
-- =============================================================================
-- Cifra TODA la base Clinical en disco (archivos de datos, log y backups),
-- de forma transparente para la aplicación (no requiere cambios de código).
--
-- Ejecutar UNA vez por un DBA con permisos de sysadmin.
--
-- ⚠️  CRÍTICO: hacé backup del certificado + su clave privada y guardalos en un
--    lugar seguro y SEPARADO del server. Sin ellos NO podés restaurar un backup
--    cifrado (perderías la base). No versiones estas claves en git.
--
-- Requiere edición de SQL Server que soporte TDE (Enterprise, o Standard 2019+).
-- =============================================================================

USE master;
GO

-- 1) Master key del servidor (omitir si ya existe).
IF NOT EXISTS (SELECT 1 FROM sys.symmetric_keys WHERE name = '##MS_DatabaseMasterKey##')
    CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'CAMBIAR__ClaveMaestraFuerte!';
GO

-- 2) Certificado que protege la Database Encryption Key.
IF NOT EXISTS (SELECT 1 FROM sys.certificates WHERE name = 'ClinicalTDECert')
    CREATE CERTIFICATE ClinicalTDECert WITH SUBJECT = 'Clinical TDE Certificate';
GO

-- 3) BACKUP del certificado + clave privada. DESCOMENTAR, ajustar rutas y ejecutar.
--    Guardá ambos archivos y las contraseñas en un lugar seguro (vault), fuera del server.
-- BACKUP CERTIFICATE ClinicalTDECert
--     TO FILE = 'C:\secure\ClinicalTDECert.cer'
--     WITH PRIVATE KEY (
--         FILE = 'C:\secure\ClinicalTDECert.pvk',
--         ENCRYPTION BY PASSWORD = 'CAMBIAR__ClaveDelBackupDelCert!'
--     );
-- GO

-- 4) Database Encryption Key + activar el cifrado.
USE Clinical;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.dm_database_encryption_keys k
    JOIN sys.databases d ON k.database_id = d.database_id
    WHERE d.name = 'Clinical')
BEGIN
    CREATE DATABASE ENCRYPTION KEY
        WITH ALGORITHM = AES_256
        ENCRYPTION BY SERVER CERTIFICATE ClinicalTDECert;
END
GO

ALTER DATABASE Clinical SET ENCRYPTION ON;
GO

-- 5) Verificación (encryption_state = 3 significa "cifrado completo").
SELECT d.name,
       d.is_encrypted,
       k.encryption_state,
       k.key_algorithm,
       k.key_length
FROM sys.databases d
LEFT JOIN sys.dm_database_encryption_keys k ON d.database_id = k.database_id
WHERE d.name = 'Clinical';
GO
