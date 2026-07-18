-- =============================================
-- Setup & Password Reset Extensions
-- Run once, after Scripts_NewModules.sql
-- =============================================

-- Add MustChangePassword to User
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[User]') AND name = 'MustChangePassword')
    ALTER TABLE [User] ADD MustChangePassword BIT NOT NULL DEFAULT 0;
GO

-- PasswordResetToken table
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PasswordResetToken')
BEGIN
    CREATE TABLE PasswordResetToken (
        TokenId    INT IDENTITY(1,1) PRIMARY KEY,
        UserId     INT          NOT NULL,
        TokenHash  NVARCHAR(64) NOT NULL,
        ExpiresAt  DATETIME2    NOT NULL,
        CreatedAt  DATETIME2    NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_PRT_User FOREIGN KEY (UserId) REFERENCES [User](UserId) ON DELETE CASCADE
    );
    CREATE INDEX IX_PRT_Hash ON PasswordResetToken (TokenHash);
END
GO

CREATE OR ALTER PROCEDURE uspSetupIsInitialized AS
BEGIN
    SELECT CAST(CASE WHEN COUNT(1) > 0 THEN 1 ELSE 0 END AS BIT) AS IsInitialized FROM [User];
END
GO

CREATE OR ALTER PROCEDURE uspSetupInit
    @Username     NVARCHAR(50),
    @Email        NVARCHAR(150),
    @PasswordHash NVARCHAR(255),
    @FirstName    NVARCHAR(100),
    @LastName     NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM [User])
    BEGIN
        RAISERROR('Sistema ya inicializado.', 16, 1); RETURN;
    END
    DECLARE @AdminRoleId INT;
    SELECT @AdminRoleId = RoleId FROM Role WHERE Name = 'Admin';
    IF @AdminRoleId IS NULL
    BEGIN
        RAISERROR('Rol Admin no encontrado.', 16, 1); RETURN;
    END
    INSERT INTO [User] (Username, Email, PasswordHash, FirstName, LastName, RoleId, State, MustChangePassword)
    VALUES (@Username, @Email, @PasswordHash, @FirstName, @LastName, @AdminRoleId, 1, 0);
END
GO

CREATE OR ALTER PROCEDURE uspPasswordResetTokenCreate
    @UserId    INT,
    @TokenHash NVARCHAR(64),
    @ExpiresAt DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM PasswordResetToken WHERE UserId = @UserId;
    INSERT INTO PasswordResetToken (UserId, TokenHash, ExpiresAt) VALUES (@UserId, @TokenHash, @ExpiresAt);
END
GO

CREATE OR ALTER PROCEDURE uspPasswordResetTokenByHash
    @TokenHash NVARCHAR(64)
AS
BEGIN
    SELECT t.TokenId, t.UserId, t.ExpiresAt, u.Username, u.Email
    FROM PasswordResetToken t
    INNER JOIN [User] u ON t.UserId = u.UserId
    WHERE t.TokenHash = @TokenHash AND t.ExpiresAt > GETUTCDATE();
END
GO

CREATE OR ALTER PROCEDURE uspPasswordResetTokenDelete @TokenId INT AS
BEGIN
    DELETE FROM PasswordResetToken WHERE TokenId = @TokenId;
END
GO

CREATE OR ALTER PROCEDURE uspUserUpdatePassword
    @UserId       INT,
    @PasswordHash NVARCHAR(255)
AS
BEGIN
    UPDATE [User] SET PasswordHash = @PasswordHash, MustChangePassword = 0 WHERE UserId = @UserId;
END
GO

-- Re-declare to include MustChangePassword + UserId in SELECTs
CREATE OR ALTER PROCEDURE uspUserByUsername @Username NVARCHAR(50) AS
BEGIN
    SELECT UserId, Username, Email, PasswordHash, FirstName, LastName,
           RoleId, DoctorId, RefreshToken, RefreshTokenExpiry, State, MustChangePassword
    FROM [User] WHERE Username = @Username AND State = 1;
END
GO

CREATE OR ALTER PROCEDURE uspUserByRefreshToken @RefreshToken NVARCHAR(255) AS
BEGIN
    SELECT UserId, Username, Email, PasswordHash, FirstName, LastName,
           RoleId, DoctorId, State, MustChangePassword
    FROM [User] WHERE RefreshToken = @RefreshToken AND State = 1;
END
GO

CREATE OR ALTER PROCEDURE uspUserRegister
    @Username           NVARCHAR(50),
    @Email              NVARCHAR(150),
    @PasswordHash       NVARCHAR(255),
    @FirstName          NVARCHAR(100),
    @LastName           NVARCHAR(100),
    @RoleId             INT,
    @MustChangePassword BIT = 1
AS
BEGIN
    INSERT INTO [User] (Username, Email, PasswordHash, FirstName, LastName, RoleId, MustChangePassword)
    VALUES (@Username, @Email, @PasswordHash, @FirstName, @LastName, @RoleId, @MustChangePassword);
END
GO
