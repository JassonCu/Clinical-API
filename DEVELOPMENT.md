# Clinical API – Guía de Desarrollo

## Tabla de contenido
1. [Arquitectura](#arquitectura)
2. [Requisitos previos](#requisitos-previos)
3. [Configuración del entorno](#configuración-del-entorno)
4. [Base de datos](#base-de-datos)
5. [Primer arranque – Setup Wizard](#primer-arranque--setup-wizard)
6. [Recuperación de contraseña](#recuperación-de-contraseña)
7. [Cambio de contraseña forzado](#cambio-de-contraseña-forzado)
8. [Seguridad CSP](#seguridad-csp)
9. [Flujo de autenticación JWT](#flujo-de-autenticación-jwt)
10. [Estructura del proyecto](#estructura-del-proyecto)

---

## Arquitectura

```
Clinical.Domain          → Entidades (User, Doctor, Patient, PasswordResetToken…)
Clinical.Interface       → Interfaces de repositorios (IAuthRepository, ISetupRepository…)
Clinical.Application.DTOS→ DTOs de request/response
Clinical.UseCases        → Handlers CQRS con MediatR (Commands y Queries)
Clinical.Persistence     → Implementaciones Dapper + SQL Server Stored Procedures
Clinical.API             → ASP.NET Core Web API (JWT Bearer)
Clinical.Web             → ASP.NET Core MVC (Cookie Auth, consume la API)
```

La Web **nunca** habla directo a la base de datos; todo pasa por la API.

---

## Requisitos previos

| Herramienta | Versión mínima |
|---|---|
| .NET SDK | 10.0 |
| SQL Server | 2019+ (Express o Docker) |
| Docker (opcional) | 20+ |

---

## Configuración del entorno

### SQL Server en Docker (recomendado para desarrollo)

```powershell
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=TuPassword123!" `
  -p 1433:1433 --name clinical_sql -d `
  mcr.microsoft.com/mssql/server:2022-latest
```

### Cadena de conexión

La contraseña **nunca** se guarda en el repositorio. Usa una variable de entorno
antes de `dotnet watch`:

```powershell
# PowerShell
$env:ConnectionStrings__ClinicalConnection = "Server=localhost,1433;Database=Clinical;User Id=sa;Password=TuPassword123!;TrustServerCertificate=true"
```

```bash
# Bash / Linux
export ConnectionStrings__ClinicalConnection="Server=localhost,1433;Database=Clinical;User Id=sa;Password=TuPassword123!;TrustServerCertificate=true"
```

El archivo `Clinical.API/appsettings.Development.json` tiene el placeholder
`CHANGE_ME` como recordatorio; la variable de entorno lo sobreescribe en tiempo
de ejecución.

---

## Base de datos

### Inicialización (primera vez)

Existe un único script maestro e idempotente que crea todo en el orden correcto:

```
Database/Scripts_Init.sql
```

**Tablas que crea (en orden de dependencia):**

```
Patient, Doctor, Analysis
  → Appointment, Exam
    → ExamResult
Role
  → [User]
    → PasswordResetToken
MedicalHistory, VitalSign, Medicine
  → Prescription → PrescriptionDetail
PatientAllergy, PatientDiagnosis
```

**Cómo ejecutarlo contra el contenedor Docker:**

```powershell
docker cp Database/Scripts_Init.sql clinical_sql:/tmp/Scripts_Init.sql

docker exec -it clinical_sql /opt/mssql-tools18/bin/sqlcmd `
  -S localhost -U sa -P "TuPassword123!" -No `
  -i /tmp/Scripts_Init.sql
```

Al terminar imprime: `Inicialización completada exitosamente.`

> El script es idempotente: usa `IF NOT EXISTS` en tablas y `CREATE OR ALTER`
> en stored procedures. Ejecutarlo de nuevo no borra datos existentes.

### Scripts individuales (referencia)

| Script | Propósito |
|---|---|
| `Scripts_StoredProcedures.sql` | Tablas base originales + SPs |
| `Scripts_NewModules.sql` | Módulos de auth, farmacia, laboratorio |
| `Scripts_Setup_Auth.sql` | Columna `MustChangePassword`, tabla `PasswordResetToken`, SPs de setup y reset |
| **`Scripts_Init.sql`** | **Combina todo lo anterior en orden correcto** ← usar este |

---

## Primer arranque – Setup Wizard

La API expone dos endpoints públicos para la inicialización:

```
GET  /api/setup/status   → { isInitialized: bool }
POST /api/setup/init     → crea el primer usuario Admin
```

`POST /api/setup/init` solo funciona cuando la tabla `[User]` está vacía.
Si ya hay usuarios devuelve `400 Bad Request`.

### Flujo en la Web

1. Al acceder a cualquier página, `AuthController.Login (GET)` llama a
   `GET /api/setup/status`.
2. Si `isInitialized == false`, redirige a `/Setup`.
3. El usuario llena el formulario con sus credenciales y datos personales.
4. La Web llama a `POST /api/setup/init`.
5. La API hashea la contraseña con **BCrypt workFactor 12** en el servidor
   (nunca viaja en plano al API) y guarda el usuario Admin.
6. Redirige al Login.

> **Seguridad:** la contraseña nunca se hardcodea en SQL ni en código.
> El hash se genera en tiempo de ejecución en el servidor .NET.

---

## Recuperación de contraseña

Patrón inspirado en Django allauth: el servidor genera el token, almacena
solo su hash SHA-256 y devuelve el raw token **una sola vez**.

### Flujo completo

```
Admin                    API                         DB
  │                        │                           │
  ├─ POST /api/user/{id}/reset-password ──────────────►│
  │                        │  genera rawToken (32 bytes aleatorios)
  │                        │  tokenHash = SHA256(rawToken) en hex
  │                        │  DELETE old tokens for userId
  │                        │  INSERT PasswordResetToken(userId, tokenHash, expiresAt=+24h)
  │◄─ { rawToken, expiresAt, targetUsername } ─────────┤
  │                        │                           │
  │  (entrega rawToken al usuario por canal seguro)
  │                        │                           │
Usuario                  API                         DB
  │                        │                           │
  ├─ POST /api/auth/reset-password ───────────────────►│
  │  { token: rawToken, newPassword }
  │                        │  recalcula SHA256(token)
  │                        │  busca en DB por hash (y ExpiresAt > now)
  │                        │  BCrypt.Hash(newPassword, workFactor=12)
  │                        │  UPDATE [User].PasswordHash
  │                        │  DELETE PasswordResetToken
  │◄─ 200 OK ──────────────┤                           │
```

**Endpoint del Admin (requiere rol Admin):**
```
POST /api/user/{userId}/reset-password
Response: { rawToken, expiresAt, targetUsername }
```

**Endpoint del usuario (público):**
```
POST /api/auth/reset-password
Body: { token, newPassword, confirmPassword }
```

En la Web, el Admin ve el token en un modal de un solo uso con botón de copiar.
El usuario accede a `/Auth/ResetPassword` y pega el token.

---

## Cambio de contraseña forzado

Cuando un Admin crea un usuario via `POST /api/auth/register`, el flag
`MustChangePassword = true` se guarda en DB.

### Flujo

1. El usuario hace login normalmente.
2. El claim `must_change_password=true` se incluye en la cookie de sesión.
3. `BaseController.OnActionExecuting` intercepta **cada request** de un
   usuario autenticado y verifica ese claim.
4. Si es `true`, redirige a `/Auth/ChangePassword` sin importar la página
   destino.
5. Al cambiar la contraseña (`POST /api/auth/change-password`), la API
   setea `MustChangePassword = 0` en DB y devuelve 200.
6. La Web renueva los claims (nuevo login) y redirige al Dashboard.

**Endpoint (requiere autenticación):**
```
POST /api/auth/change-password
Body: { newPassword, confirmPassword }
```

---

## Seguridad CSP

El middleware `SecurityHeadersMiddleware` aplica las siguientes cabeceras en
**cada request**:

| Cabecera | Valor |
|---|---|
| `X-Content-Type-Options` | `nosniff` |
| `X-Frame-Options` | `DENY` |
| `X-XSS-Protection` | `1; mode=block` |
| `Referrer-Policy` | `strict-origin-when-cross-origin` |
| `Permissions-Policy` | `geolocation=(), microphone=(), camera=()` |
| `Content-Security-Policy` | ver abajo |

### Content-Security-Policy

```
default-src 'self';
script-src  'self' 'nonce-{nonce}' https://cdn.jsdelivr.net https://cdnjs.cloudflare.com
            https://code.jquery.com https://cdn.datatables.net;
style-src   'self' https://cdn.jsdelivr.net https://cdnjs.cloudflare.com
            https://fonts.googleapis.com https://cdn.datatables.net;
font-src    'self' https://cdn.jsdelivr.net https://fonts.gstatic.com https://cdnjs.cloudflare.com;
img-src     'self' data:;
connect-src 'self' https://cdn.jsdelivr.net [+ ws://localhost:* en Development];
```

**No se usa `'unsafe-inline'`** ni en `script-src` ni en `style-src`.

### Reglas para nuevas vistas

- **No usar `<style>...</style>`** en las vistas. Los estilos van en:
  - `wwwroot/css/site.css` (vistas con layout)
  - `wwwroot/css/auth-pages.css` (páginas standalone: Login, Setup, ResetPassword, ChangePassword)

- **No usar `style="..."` en línea**. Usar clases de utilidad definidas en `site.css`:

  | Clase | Equivalente inline |
  |---|---|
  | `text-xs` | `font-size:.85rem` |
  | `text-sm` | `font-size:.875rem` |
  | `text-2xs` | `font-size:.75rem` |
  | `ls-tight` | `letter-spacing:.4px` |
  | `min-vh-60` | `min-height:60vh` |
  | `mw-form-md` | `max-width:500px` |
  | `mw-col-sm` | `max-width:150px` |
  | `denied-icon` | `font-size:4rem;color:#dc3545` |
  | `entity-avatar` | avatar cuadrado azul 56px |
  | `entity-avatar-green` | avatar cuadrado verde 56px |
  | `sidebar-avatar` | avatar circular sidebar 32px |
  | `sidebar-username` | texto nombre en sidebar |
  | `sidebar-role` | texto rol en sidebar |
  | `sidebar-logout` | botón logout sidebar |
  | `stat-accent-primary/success/info/danger/warning` | borde izquierdo stat-card |

- **No usar `onclick="..."` ni `<script>` inline**. Los scripts van en:
  - `wwwroot/js/site.js` (global)
  - `wwwroot/js/login.js` (página de login)
  - `wwwroot/js/token-modal.js` (modal de token en usuarios)
  - Scripts de página específica: declarar en `@section Scripts { <script src="~/js/mi-script.js"></script> }`

- Los scripts externos no necesitan nonce (solo los inline, que no deben existir).

---

## Flujo de autenticación JWT

```
Clinical.Web (Cookie)  ──► Clinical.API (JWT Bearer)  ──► SQL Server
```

1. El usuario hace login en la Web.
2. La Web llama a `POST /api/auth/login` → recibe `accessToken` y `refreshToken`.
3. La Web guarda los tokens en **cookies HttpOnly** (`X-Clinical-Token`, `X-Clinical-Refresh`).
4. La Web crea una cookie de sesión MVC con los claims del usuario.
5. `TokenRefreshMiddleware` en la Web intercepta requests, verifica expiración
   del accessToken y lo refresca automáticamente con `POST /api/auth/refresh`.

### Claims en la cookie MVC

| Claim | Fuente |
|---|---|
| `ClaimTypes.NameIdentifier` | Username |
| `ClaimTypes.Name` | Username |
| `ClaimTypes.Email` | Email |
| `full_name` | `$"{FirstName} {LastName}"` |
| `ClaimTypes.Role` | Nombre del rol |
| `token_expiry` | DateTime ISO-8601 |
| `user_id` | UserId (int) |
| `must_change_password` | `"true"` / `"false"` |

---

## Cómo correr el proyecto

```powershell
# Terminal 1 – API
$env:ConnectionStrings__ClinicalConnection = "Server=localhost,1433;Database=Clinical;User Id=sa;Password=TuPassword!;TrustServerCertificate=true"
dotnet watch run --project Clinical.API

# Terminal 2 – Web
dotnet watch run --project Clinical.Web
```

| URL | Propósito |
|---|---|
| `http://localhost:5123` | API (Swagger en Development) |
| `http://localhost:5124` | Web MVC |
| `http://localhost:5124/Setup` | Setup wizard (solo si DB vacía) |
| `http://localhost:5124/Auth/Login` | Login |
| `http://localhost:5124/Auth/ResetPassword` | Restablecer contraseña con token |
