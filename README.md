# Clinical System

Sistema de gestión clínica compuesto por dos proyectos:

| Proyecto | Descripción | Puerto |
|---|---|---|
| **Clinical.API** | REST API — .NET 10, Clean Architecture, CQRS, Dapper | `5123` |
| **Clinical.Web** | Frontend MVC — ASP.NET Core 10 MVC, Bootstrap 5.3 | `5124` |

---

## Tabla de Contenidos

- [Arquitectura](#arquitectura)
- [Stack tecnológico](#stack-tecnológico)
- [Correr en local](#correr-en-local)
  - [Requisitos previos](#requisitos-previos)
  - [1. Clonar el repositorio](#1-clonar-el-repositorio)
  - [2. Levantar SQL Server en Docker](#2-levantar-sql-server-en-docker)
  - [3. Crear el esquema — orden de ejecución de scripts](#3-crear-el-esquema--orden-de-ejecución-de-scripts)
  - [4. Configurar la cadena de conexión](#4-configurar-la-cadena-de-conexión)
  - [5. Iniciar la API](#5-iniciar-la-api)
  - [6. Crear el primer usuario Admin](#6-crear-el-primer-usuario-admin)
  - [7. Iniciar el Frontend](#7-iniciar-el-frontend)
- [Correr con Docker](#correr-con-docker)
- [Configuración](#configuración)
- [Seguridad](#seguridad)
- [Módulos y Endpoints (API)](#módulos-y-endpoints-api)
- [Flujo de autenticación](#flujo-de-autenticación)
- [Base de datos](#base-de-datos)
- [Estructura del proyecto](#estructura-del-proyecto)

---

## Arquitectura

### Sistema completo

```
┌─────────────────────────────┐       ┌──────────────────────────────────────┐
│      Clinical.Web            │       │           Clinical.API                │
│   ASP.NET Core MVC           │  HTTP │   ASP.NET Core Web API               │
│   Bootstrap 5.3              │──────>│   13 controladores REST               │
│   Cookie Auth + JWT Cookies  │       │   JWT Bearer Auth                    │
│   Razor Views                │       │   Rate limiting · CORS               │
└─────────────────────────────┘       └──────────────┬───────────────────────┘
                                                       │
                                       ┌───────────────▼───────────────────────┐
                                       │         Clinical.UseCases              │
                                       │  Commands · Queries · Handlers         │
                                       │  FluentValidation · AutoMapper         │
                                       └───────────────┬───────────────────────┘
                                                       │
                             ┌─────────────────────────┼────────────────────────┐
                             │                         │                        │
                  ┌──────────▼──────────┐  ┌──────────▼──────────┐  ┌─────────▼───────┐
                  │  Clinical.Interface  │  │   Clinical.Domain    │  │  Clinical.DTOs  │
                  │  IUnitOfWork         │  │   14 entidades POCO  │  │  Request/Resp.  │
                  │  IGenericRepository  │  └─────────────────────┘  └─────────────────┘
                  └──────────┬──────────┘
                             │
                  ┌──────────▼──────────┐   ┌──────────────────────┐
                  │ Clinical.Persistence │   │ Clinical.Infraestruct.│
                  │ Dapper + SQL Server  │   │ JwtTokenService       │
                  │ Stored Procedures    │   └──────────────────────┘
                  └─────────────────────┘
```

### Clean Architecture — API

La API sigue **Clean Architecture** con separación estricta de capas y el patrón **CQRS** vía MediatR.

| Decisión | Justificación |
|---|---|
| Dapper (sin EF Core) | Control fino sobre SQL; mejor rendimiento en queries de reporte |
| Solo Stored Procedures | Todo acceso a DB pasa por SPs — sin SQL inline en la aplicación |
| MediatR CQRS | Commands y Queries completamente separados; handlers delgados y enfocados |
| Unit of Work + Generic Repo | Acceso a datos consistente; repos especializados solo para JOINs complejos |
| JWT + Refresh Token | Auth stateless con tokens de corta vida y rotación de refresh tokens |

### Clean Architecture — Web MVC

El frontend MVC también sigue Clean Architecture:

```
Clinical.Web/
├── Core/               ← DTOs, Interfaces (contratos), Modelos
├── Infrastructure/     ← Implementaciones HttpClient de los servicios
├── Controllers/        ← Capa de presentación MVC
├── Middleware/         ← SecurityHeaders, TokenRefresh
└── Views/              ← Razor views (Bootstrap 5.3)
```

---

## Stack Tecnológico

### Clinical.API

| Capa | Tecnología | Versión |
|---|---|---|
| Runtime | .NET | 10.0 |
| Web Framework | ASP.NET Core | 10.0 |
| ORM / Data Access | Dapper | 2.1.35 |
| Base de datos | SQL Server | 2022 |
| CQRS Mediator | MediatR | 12.4.1 |
| Validación | FluentValidation | 11.11.0 |
| Mapeo | AutoMapper | 12.0.1 |
| Auth | JWT Bearer | 10.0.0 |
| Hashing de contraseñas | BCrypt.Net-Next (work factor 12) | 4.0.3 |
| Logging | Serilog | 9.0.0 |
| Documentación API | Scalar (OpenAPI) | — |
| Testing | xUnit + Moq | — |

### Clinical.Web

| Tecnología | Uso |
|---|---|
| ASP.NET Core 10 MVC | Framework web principal |
| Razor Views | Motor de plantillas server-side |
| Bootstrap 5.3 | Componentes UI responsive |
| Bootstrap Icons 1.11 | Iconografía |
| DataTables 1.13 | Tablas con paginación y búsqueda |
| SweetAlert2 | Diálogos de confirmación |
| Serilog | Logging estructurado |
| IHttpClientFactory | Clientes HTTP tipados hacia la API |
| Cookie Authentication | Gestión de sesión en el navegador |

---

## Correr en Local

> Guía end-to-end para levantar el sistema en una máquina nueva usando **SQL Server en Docker**. Es el camino recomendado y el que usa el equipo. Si preferís una instancia nativa de SQL Server/SQLEXPRESS, sólo cambia el paso 2 y el `Server=...` de la cadena de conexión.

### Requisitos Previos

| Herramienta | Uso | Verificar |
|---|---|---|
| [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) | Compilar y correr API + Web | `dotnet --version` → `10.x.x` |
| [Docker Desktop](https://www.docker.com/products/docker-desktop/) | SQL Server en contenedor | `docker --version` |
| Git | Clonar el repo | `git --version` |

> `sqlcmd` **no** es necesario en el host: los scripts se ejecutan **dentro** del contenedor con `docker exec`. Si preferís, podés usar Azure Data Studio / SSMS conectándote a `localhost,1433`.

---

### 1. Clonar el Repositorio

```bash
git clone https://github.com/JassonCu/Clinical-API.git
cd Clinical-API
```

---

### 2. Levantar SQL Server en Docker

Crea el contenedor **una sola vez**. Elegí tu propia contraseña fuerte para `sa` (mínimo 8 caracteres, con mayúscula, minúscula, número y símbolo) y **recordala** para el paso 4.

```bash
docker run -d \
  --name clinical_sql \
  -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=TuPassword_Fuerte123" \
  -e "MSSQL_PID=Developer" \
  -p 1433:1433 \
  mcr.microsoft.com/mssql/server:2022-latest
```

Verificá que quedó arriba y que el puerto está mapeado:

```bash
docker ps --filter "name=clinical_sql" --format "{{.Names}} | {{.Status}} | {{.Ports}}"
# clinical_sql | Up ... | 0.0.0.0:1433->1433/tcp
```

> Si el contenedor ya existe y está detenido: `docker start clinical_sql`.
> Windows: si `\` de multilínea da problemas en PowerShell, poné todo el `docker run` en una sola línea.

---

### 3. Crear el Esquema — Orden de Ejecución de Scripts

Los scripts SQL están en la carpeta [`Database/`](Database/). **Hay dos caminos:**

#### ✅ Camino recomendado (instalación nueva): un solo script

[`Scripts_Init.sql`](Database/Scripts_Init.sql) es el **script maestro consolidado e idempotente**: crea la base `Clinical`, las **16 tablas**, los **96 stored procedures** y siembra los roles. Incluye todo lo de los scripts incrementales (auth, fix de estados, detalles de receta). Es seguro correrlo varias veces (`CREATE OR ALTER` + `IF NOT EXISTS`).

```bash
# Copiar el script al contenedor y ejecutarlo
docker cp Database/Scripts_Init.sql clinical_sql:/tmp/Scripts_Init.sql
docker exec clinical_sql /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "TuPassword_Fuerte123" -C \
  -i /tmp/Scripts_Init.sql
```

> Con esto la base queda **100% lista**. Saltá al paso 4.
> Nota: en imágenes viejas la ruta de sqlcmd es `/opt/mssql-tools/bin/sqlcmd` (sin el `18`).

#### 🔧 Camino incremental (referencia / actualizar una base antigua)

Sólo si necesitás aplicar los cambios uno por uno sobre una base preexistente. **El orden importa** (cada script depende de objetos creados por el anterior):

| # | Script | Crea / Modifica | Depende de |
|---|--------|-----------------|------------|
| 1 | `Scripts_StoredProcedures.sql` | Tablas base: `Patient`, `Doctor`, `Appointment`, `Analysis`, `Exam`, `ExamResult` (+31 SPs) | — |
| 2 | `Scripts_NewModules.sql` | `Role`, `User`, `MedicalHistory`, `VitalSign`, `Medicine`, `Prescription`, `PrescriptionDetail`, `PatientAllergy`, `PatientDiagnosis` (+50 SPs) + siembra roles | 1 |
| 3 | `Scripts_Setup_Auth.sql` | Columna `MustChangePassword`, tabla `PasswordResetToken` + SPs de reseteo/setup | 2 (altera `[User]`) |
| 4 | `Scripts_Fix_State.sql` | Agrega `@State` a SPs de registro + crea `uspAnalysisRegister` / `uspExamRegister` | 1, 2 |
| 5 | `Scripts_Prescription_Details.sql` | `uspPrescriptionRegister` (devuelve `SCOPE_IDENTITY`) + `uspPrescriptionDetailRegister` — *Feature: detalles de receta* | 2 |
| 6 | `Scripts_Encryption_TDE.sql` | **Opcional / sólo producción.** Cifrado en reposo (TDE). Lo ejecuta un DBA. | Base creada |

```bash
# Ejemplo aplicando el orden incremental dentro del contenedor
for f in Scripts_StoredProcedures Scripts_NewModules Scripts_Setup_Auth Scripts_Fix_State Scripts_Prescription_Details; do
  docker cp "Database/$f.sql" clinical_sql:/tmp/$f.sql
  docker exec clinical_sql /opt/mssql-tools18/bin/sqlcmd \
    -S localhost -U sa -P "TuPassword_Fuerte123" -C -d Clinical -i /tmp/$f.sql
done
```

> Los roles sembrados por defecto son: `Admin`, `Doctor`, `Nurse`, `Pharmacist`, `Receptionist`.
> `Scripts_Encryption_TDE.sql` **no** se aplica en local (requiere edición Enterprise/Standard 2019+ y gestión de certificados).

Comprobá que quedó todo:

```bash
docker exec clinical_sql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "TuPassword_Fuerte123" -C -d Clinical \
  -Q "SELECT CONCAT('Tablas=', (SELECT COUNT(*) FROM sys.tables), '  SPs=', (SELECT COUNT(*) FROM sys.procedures));"
# Esperado: Tablas=16  SPs=96
```

---

### 4. Configurar la Cadena de Conexión

Poné la cadena de conexión en **`Clinical.API/appsettings.Development.json`**. Este archivo **está en `.gitignore`** (no se versiona), justamente porque contiene la contraseña del `sa`. Como se lee desde la carpeta del proyecto, funciona en cualquier terminal sin depender del perfil de usuario de Windows.

Si el archivo no existe en tu copia (recién clonada), crealo con este contenido:

```json
{
  "Logging": {
    "LogLevel": { "Default": "Information", "Microsoft.AspNetCore": "Warning" }
  },
  "ConnectionStrings": {
    "ClinicalConnection": "Server=localhost,1433;Database=Clinical;User Id=sa;Password=TuPassword_Fuerte123;TrustServerCertificate=true;Encrypt=false"
  },
  "JwtSettings": {
    "SecretKey": "DEV-ONLY-LocalDevelopmentSigningKey-DoNotUseInProduction-0123456789"
  }
}
```

> Reemplazá `TuPassword_Fuerte123` por la **misma** contraseña del paso 2.
> La API **falla al arrancar** con un mensaje claro si no encuentra la cadena (`ConnectionStrings:ClinicalConnection`) — es intencional (fail-fast).
> **Alternativa** sin tocar el archivo: exportar la variable de entorno `ConnectionStrings__ClinicalConnection` con el mismo valor (útil en CI o si preferís no tener credenciales en disco).
> En **producción** la cadena y la clave JWT vienen siempre de variables de entorno, nunca de archivos versionados.

---

### 5. Iniciar la API

```bash
dotnet run --project Clinical.API
```

La API queda disponible en:

| Recurso | URL |
|---|---|
| API base | `http://localhost:5123` |
| Documentación interactiva (Scalar) | `http://localhost:5123/scalar/v1` |
| Health check | `http://localhost:5123/health` |
| OpenAPI JSON | `http://localhost:5123/openapi/v1.json` |

---

### 6. Crear el Primer Usuario Admin

En una base recién creada **no hay usuarios**. La API expone un endpoint de setup para crear el primer administrador (sólo funciona si aún no hay usuarios).

```bash
# ¿Ya está inicializado el sistema?
curl http://localhost:5123/api/setup/status

# Crear el primer admin
curl -X POST http://localhost:5123/api/setup/init \
  -H "Content-Type: application/json" \
  -d '{
        "username": "admin",
        "email": "admin@clinical.com",
        "password": "Admin123!",
        "firstName": "Admin",
        "lastName": "Sistema"
      }'
```

Probá el login:

```bash
curl -X POST http://localhost:5123/api/auth/Login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Admin123!"}'
```

Respuesta esperada (200):

```json
{
  "accessToken": "eyJhbGci...",
  "refreshToken": "base64...",
  "username": "admin",
  "fullName": "Admin Sistema",
  "role": "Admin",
  "mustChangePassword": false
}
```

> Un **401** con `"El usuario y/o contraseña es incorrecta"` significa que la conexión a la BD funciona pero las credenciales no coinciden.
> Un **500** con `SqlException ... error 26` significa que la API no llega al contenedor: revisá que `clinical_sql` esté arriba y que la cadena del paso 4 (`appsettings.Development.json`) tenga la contraseña correcta.

---

### 7. Iniciar el Frontend

Config ya presente en `Clinical.Web/appsettings.Development.json`:

```json
{
  "ApiSettings": {
    "BaseUrl": "http://localhost:5123"
  }
}
```

En una **segunda terminal** (la API debe estar corriendo):

```bash
dotnet run --project Clinical.Web
```

El frontend queda disponible en `http://localhost:5124`. Iniciá sesión con el usuario Admin del paso 6; el sistema redirige al dashboard.

> **Nota:** La API debe estar corriendo antes de iniciar el frontend.

---

## Correr con Docker

### Prerequisitos

- Docker Engine + Docker Compose

### Configuración

Crea un archivo `.env` en la raíz del proyecto:

```env
SA_PASSWORD=YourStrong!Passw0rd
JWT_SECRET_KEY=cambia-esto-por-al-menos-64-caracteres-aleatorios-para-produccion
```

### Levantar todo

```bash
docker compose up --build
```

Servicios disponibles:

| Servicio | URL |
|---|---|
| Frontend (Clinical.Web) | `http://localhost:5124` |
| API (Clinical.API) | `http://localhost:5123` |
| SQL Server | `localhost:1433` |

El orden de arranque es: `sqlserver` → `clinicalapi` → `clinicalweb`.
SQL Server tiene un health check; la API espera a que el check pase antes de arrancar.
Los datos de SQL Server se persisten en el volumen `sqlserver_data`.

### Detener

```bash
docker compose down
# Para eliminar también el volumen (borra todos los datos):
docker compose down -v
```

---

## Configuración

### Variables de entorno — API

| Variable | Descripción |
|---|---|
| `ConnectionStrings__ClinicalConnection` | Cadena de conexión a SQL Server |
| `JwtSettings__SecretKey` | Secreto de firma JWT (mín. 32 chars) |
| `JwtSettings__AccessTokenExpiryMinutes` | TTL del access token (default: 15) |
| `JwtSettings__RefreshTokenExpiryDays` | TTL del refresh token (default: 7) |
| `ASPNETCORE_ENVIRONMENT` | `Development` / `Production` |

### Variables de entorno — Web

| Variable | Descripción |
|---|---|
| `ApiSettings__BaseUrl` | URL base de la API (ej: `http://localhost:5123`) |
| `ASPNETCORE_ENVIRONMENT` | `Development` / `Production` |

### Logging

Serilog escribe logs estructurados en:

- **Consola** — durante el tiempo de ejecución
- **`logs/clinical-YYYYMMDD.log`** — archivos diarios rotantes (API)
- **`logs/clinical-web-YYYYMMDD.log`** — archivos diarios rotantes (Web)

---

## Seguridad

### API

| Medida | Detalle |
|---|---|
| **JWT Bearer** | Tokens firmados con `HmacSha256`; `ClockSkew: Zero` |
| **Access token** | 15 minutos de vida |
| **Refresh token** | 64 bytes aleatorios (Base64), 7 días; **rotación en cada uso** |
| **BCrypt** | Work factor 12 para hashing de contraseñas |
| **Rate limiting** | 5 req/min en auth, 100 req/min global (por IP) |
| **CORS** | Dev: abierto a orígenes configurados; Prod: métodos y headers restringidos |
| **Security headers** | `X-Content-Type-Options`, `X-Frame-Options: DENY`, `X-XSS-Protection`, `Referrer-Policy`, `Permissions-Policy`, HSTS (solo producción) |
| **Role-based auth** | `Admin`, `Doctor`, `Nurse`, `Pharmacist`, `Receptionist` |

### Frontend MVC

| Medida | Detalle |
|---|---|
| **JWT en cookies HttpOnly** | `X-Clinical-Token` y `X-Clinical-Refresh` — nunca en `localStorage` |
| **Cookie de sesión** | `HttpOnly` + `Secure` + `SameSite=Strict` |
| **CSRF** | `AutoValidateAntiforgeryToken` global + `@Html.AntiForgeryToken()` en cada formulario |
| **Token refresh automático** | Middleware renueva el JWT 2 minutos antes de expirar sin interrumpir la sesión; redirige a login si el refresh falla |
| **Rate limiting** | 5 req/min en login, 200 req/min global |
| **CSP + Security headers** | `Content-Security-Policy`, `X-Frame-Options: DENY`, `X-XSS-Protection`, `Referrer-Policy`, `Permissions-Policy` |
| **Autorización por rol** | `[Authorize(Roles="Admin,Pharmacist")]` en endpoints sensibles |
| **Validación de entrada** | DataAnnotations en todos los ViewModels |
| **XSS** | Razor codifica automáticamente toda la salida HTML |

---

## Módulos y Endpoints (API)

Todos los endpoints (excepto `POST /api/auth/Login` y `POST /api/auth/RefreshToken`) requieren `Authorization: Bearer <token>`.

### Auth — `/api/auth`

| Método | Ruta | Auth | Descripción |
|---|---|---|---|
| POST | `/Login` | Público | Autenticar usuario, retorna access + refresh token |
| POST | `/Register` | Admin | Registrar nuevo usuario del sistema |
| POST | `/RefreshToken` | Público | Rotar refresh token, retorna nuevo par de tokens |

### Pacientes — `/api/patient`

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/` | Listar todos los pacientes |
| GET | `/{patientId}` | Obtener paciente por ID |
| POST | `/Register` | Crear paciente |
| PUT | `/Edit` | Actualizar paciente |
| DELETE | `/Remove/{patientId}` | Eliminar paciente |
| PATCH | `/ChangeState` | Activar / desactivar |

### Médicos — `/api/doctor`

Misma estructura que Paciente. Campos adicionales: `Subspecialty`, `ConsultationFee`, `WorkingSchedule`, `Biography`.

### Citas — `/api/appointment`

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/` | Listar todas las citas |
| GET | `/{appointmentId}` | Obtener por ID |
| GET | `/ByPatient/{patientId}` | Filtrar por paciente |
| GET | `/ByDoctor/{doctorId}` | Filtrar por médico |
| POST | `/Register` | Agendar cita |
| PUT | `/Edit` | Actualizar cita |
| DELETE | `/Remove/{appointmentId}` | Eliminar |
| PATCH | `/ChangeState` | Cambiar estado |

### Historia Clínica — `/api/medicalhistory`

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/{medicalHistoryId}` | Obtener por ID |
| GET | `/ByPatient/{patientId}` | Historia del paciente |
| POST | `/Register` | Crear registro |
| PUT | `/Edit` | Actualizar |
| DELETE | `/Remove/{medicalHistoryId}` | Solo Admin |

Campos: `BloodType`, `ChronicDiseases`, `PreviousSurgeries`, `FamilyHistory`, `CurrentMedications`, `Habits`, `Observations`.

### Signos Vitales — `/api/vitalsign`

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/` | Listar todos los registros |
| GET | `/ByPatient/{patientId}` | Filtrar por paciente |
| POST | `/Register` | Registrar signos (IMC calculado automáticamente) |
| DELETE | `/Remove/{vitalSignId}` | Solo Admin |

Campos: `Weight`, `Height`, `BMI` *(auto-calculado)*, `BloodPressure`, `HeartRate`, `Temperature`, `OxygenSaturation`, `RespiratoryRate`, `GlucoseLevel`.

### Medicamentos / Farmacia — `/api/medicine`

| Método | Ruta | Auth | Descripción |
|---|---|---|---|
| GET | `/` | Autenticado | Listar medicamentos |
| GET | `/{medicineId}` | Autenticado | Obtener por ID |
| GET | `/LowStock` | Autenticado | Medicamentos bajo stock mínimo |
| POST | `/Register` | Admin, Pharmacist | Crear medicamento |
| PUT | `/Edit` | Admin, Pharmacist | Actualizar |
| DELETE | `/Remove/{medicineId}` | Admin | Eliminar |
| PATCH | `/ChangeState` | Admin, Pharmacist | Activar / desactivar |

### Recetas — `/api/prescription`

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/` | Listar todas las recetas |
| GET | `/{prescriptionId}` | Obtener con líneas de detalle |
| GET | `/ByPatient/{patientId}` | Recetas del paciente |
| GET | `/ByDoctor/{doctorId}` | Recetas del médico |
| POST | `/Register` | Crear receta |
| DELETE | `/Remove/{prescriptionId}` | Solo Admin |
| PATCH | `/ChangeState` | Cambiar estado (ACTIVA / DISPENSADA / VENCIDA / CANCELADA) |

### Alergias — `/api/patientallergy`

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/` | Listar todos los registros |
| GET | `/{allergyId}` | Obtener por ID |
| GET | `/ByPatient/{patientId}` | Alergias del paciente |
| POST | `/Register` | Registrar alergia |
| PUT | `/Edit` | Actualizar |
| DELETE | `/Remove/{allergyId}` | Solo Admin |
| PATCH | `/ChangeState` | Activar / desactivar |

Valores de severidad: `Leve`, `Moderada`, `Grave`, `Anafilaxia`.

### Diagnósticos — `/api/patientdiagnosis`

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/` | Listar todos |
| GET | `/{diagnosisId}` | Obtener por ID |
| GET | `/ByPatient/{patientId}` | Diagnósticos del paciente |
| GET | `/ByAppointment/{appointmentId}` | Diagnósticos de una cita |
| POST | `/Register` | Registrar diagnóstico (con código CIE-10) |
| DELETE | `/Remove/{diagnosisId}` | Solo Admin |
| PATCH | `/ChangeState` | Activar / desactivar |

### Exámenes — `/api/exam`

CRUD completo para el catálogo de exámenes. Create / Read / Update / Delete / ChangeState.

### Resultados de Exámenes — `/api/examresult`

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/` | Listar todos los resultados |
| GET | `/{examResultId}` | Obtener por ID |
| GET | `/ByPatient/{patientId}` | Resultados del paciente |
| GET | `/ByAppointment/{appointmentId}` | Resultados de una cita |
| POST | `/Register` | Registrar resultado |
| PUT | `/Edit` | Actualizar |
| DELETE | `/Remove/{id}` | Eliminar |
| PATCH | `/ChangeState` | Cambiar estado |

### Análisis — `/api/analysis`

CRUD completo para el catálogo de tipos de análisis.

---

## Flujo de Autenticación

### En la API (Bearer Token)

```
Cliente                             API
  │                                  │
  │  POST /api/auth/Login             │
  │  { username, password }           │
  │──────────────────────────────────>│
  │                                  │  BCrypt.Verify(password, hash)
  │                                  │  GenerateAccessToken → JWT 15 min
  │                                  │  GenerateRefreshToken → 64 bytes Base64
  │  200 { accessToken,               │  Guarda refreshToken en [User]
  │        refreshToken,              │
  │        expiresAt, role }          │
  │<─────────────────────────────────│
  │                                  │
  │  GET /api/patient                 │
  │  Authorization: Bearer <jwt>      │
  │──────────────────────────────────>│  Valida JWT (issuer, audience,
  │  200 { data }                    │  firma, lifetime, ClockSkew=0)
  │<─────────────────────────────────│
  │                                  │
  │  POST /api/auth/RefreshToken      │
  │  { refreshToken }                 │
  │──────────────────────────────────>│  Busca token en [User]
  │                                  │  Verifica RefreshTokenExpiry > UtcNow
  │  200 { nuevo accessToken,         │  Emite nuevos tokens
  │        nuevo refreshToken }      │  Invalida el refresh token anterior
  │<─────────────────────────────────│
```

### En el Frontend MVC (Cookies HttpOnly)

```
Navegador                       Clinical.Web                    Clinical.API
    │                                │                               │
    │  POST /Auth/Login               │                               │
    │  (form + AntiForgery token)     │                               │
    │────────────────────────────────>│                               │
    │                                │  POST /api/auth/Login ────────>│
    │                                │<───────── { accessToken, ... }─│
    │                                │  Crea ClaimsPrincipal          │
    │                                │  Cookie: ClinicalWeb.Auth      │
    │  Set-Cookie: X-Clinical-Token   │  Cookie: X-Clinical-Token      │
    │  Set-Cookie: X-Clinical-Refresh │  Cookie: X-Clinical-Refresh    │
    │<────────────────────────────────│                               │
    │                                │                               │
    │  GET /Patient (cualquier página)│                               │
    │────────────────────────────────>│                               │
    │                                │  Lee X-Clinical-Token de cookie│
    │                                │  GET /api/patient ────────────>│
    │                                │  Authorization: Bearer <token>  │
    │                                │<─────────── 200 { data } ──────│
    │<────── HTML renderizado ────────│                               │
    │                                │                               │
    │  (Token a 2 min de vencer)      │                               │
    │────────────────────────────────>│  TokenRefreshMiddleware detecta│
    │                                │  POST /api/auth/RefreshToken ─>│
    │                                │<─────── nuevos tokens ─────────│
    │                                │  Actualiza cookies             │
    │<────── respuesta normal ────────│                               │
```

---

## Base de Datos

### Convención de Stored Procedures

Todos los SPs siguen el patrón `usp{Módulo}{Acción}`:

| Procedimiento | Descripción |
|---|---|
| `uspPatientList` | Listar todos los pacientes |
| `uspPatientById` | Obtener paciente por ID |
| `uspMedicalHistoryByPatient` | Historia clínica por paciente |
| `uspPrescriptionById` | Cabecera + líneas de detalle (multi-map JOIN) |
| `uspMedicineLowStock` | Medicamentos con `CurrentStock <= MinimumStock` |
| `uspVitalSignRegister` | Insertar signos vitales con cálculo de IMC |
| `uspUserByUsername` | Obtener usuario para autenticación |
| `uspUserUpdateRefreshToken` | Rotar refresh token |

### Tablas principales

| Tabla | Descripción |
|---|---|
| `Role` | Roles del sistema (5 roles seeded) |
| `[User]` | Usuarios con hash BCrypt y refresh token |
| `Patient` | Datos demográficos del paciente |
| `Doctor` | Datos del médico y especialidad |
| `Appointment` | Citas médicas |
| `MedicalHistory` | Historia clínica del paciente |
| `VitalSign` | Mediciones de signos vitales por cita |
| `Medicine` | Catálogo de medicamentos con control de stock |
| `Prescription` | Recetas médicas |
| `PrescriptionDetail` | Líneas de medicamentos por receta |
| `PatientAllergy` | Alergias del paciente con clasificación de severidad |
| `PatientDiagnosis` | Diagnósticos por cita con códigos CIE-10 |
| `Exam` | Catálogo de tipos de examen |
| `Analysis` | Tipos de análisis |
| `ExamResult` | Resultados de exámenes por paciente/cita |

---

## Estructura del Proyecto

```
Clinical-API/
│
├── Clinical.API/                        # Punto de entrada — REST API
│   ├── Controllers/                     # 13 controladores (uno por módulo)
│   ├── Extensions/Middleware/           # Middleware de manejo global de errores
│   ├── appsettings.json                 # Configuración base
│   ├── appsettings.Development.json     # Overrides de desarrollo
│   ├── Dockerfile                       # Imagen Docker del API
│   └── Program.cs                       # Bootstrap (auth, CORS, rate limiting, Serilog)
│
├── Clinical.Web/                        # Frontend MVC
│   ├── Controllers/                     # 15 controladores MVC
│   │   ├── BaseController.cs            # Controlador base con helpers
│   │   ├── AuthController.cs            # Login, Logout, AccessDenied
│   │   ├── DashboardController.cs       # Dashboard con estadísticas
│   │   └── (un controlador por módulo)
│   ├── Core/
│   │   ├── DTOs/                        # DTOs por módulo (un archivo por módulo)
│   │   ├── Interfaces/                  # Contratos de servicio (IAuthService, …)
│   │   └── Models/                      # ApiResponse<T>, UserSessionInfo
│   ├── Infrastructure/
│   │   └── Services/                    # Implementaciones HttpClient
│   │       ├── BaseApiService.cs        # GET/POST/PUT/PATCH/DELETE con auth header
│   │       └── (un servicio por módulo)
│   ├── Middleware/
│   │   ├── SecurityHeadersMiddleware.cs # CSP, X-Frame-Options, etc.
│   │   └── TokenRefreshMiddleware.cs    # Renovación automática de JWT
│   ├── Extensions/
│   │   └── ServiceExtensions.cs         # Registro de DI, auth, rate limiting
│   ├── Views/
│   │   ├── Shared/
│   │   │   ├── _Layout.cshtml           # Layout principal con sidebar y navbar
│   │   │   ├── _Alerts.cshtml           # Alertas flotantes de TempData
│   │   │   └── Error.cshtml
│   │   ├── Auth/                        # Login, AccessDenied
│   │   ├── Dashboard/                   # Estadísticas y últimas actividades
│   │   ├── Patient/                     # Index, Create, Edit, Details
│   │   ├── Doctor/                      # Index, Create, Edit, Details
│   │   ├── Appointment/                 # Index, Create, Edit, Details
│   │   ├── Medicine/                    # Index, Create, Edit, LowStock
│   │   ├── Prescription/                # Index, Create, Details
│   │   ├── VitalSign/                   # Index, Create, ByPatient
│   │   ├── MedicalHistory/              # ByPatient, Create, Edit
│   │   ├── PatientAllergy/              # Index, Create, Edit, ByPatient
│   │   ├── PatientDiagnosis/            # Index, Create
│   │   ├── Exam/                        # Index, Create
│   │   ├── Analysis/                    # Index, Create, Edit
│   │   └── ExamResult/                  # Index, Create, Details
│   ├── wwwroot/
│   │   ├── css/site.css                 # Estilos del sistema (sidebar, cards, forms)
│   │   └── js/site.js                   # DataTables init, SweetAlert confirm, receta builder
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── Dockerfile                       # Imagen Docker del frontend
│   └── Program.cs                       # Bootstrap con cookie auth, antiforgery, rate limiting
│
├── Clinical.UseCases/                   # Lógica de negocio — CQRS
│   ├── UseCases/
│   │   ├── Auth/Commands/               # Login · Register · RefreshToken
│   │   ├── Patient/                     # Commands + Queries
│   │   ├── Doctor/ · Appointment/
│   │   ├── MedicalHistory/ · VitalSign/
│   │   ├── Medicine/ · Prescription/
│   │   ├── PatientAllergy/ · PatientDiagnosis/
│   │   ├── Exam/ · ExamResult/ · Analysis/
│   │   └── (Commands: Create/Update/Delete/ChangeState + Handlers + Validators)
│   │       (Queries: GetAll/GetById/GetBy* + Handlers)
│   ├── Mapping/                         # Perfiles AutoMapper (uno por módulo)
│   └── Commons/Behaviours/              # Pipeline de validación FluentValidation
│
├── Clinical.Domain/                     # Entidades de dominio (POCOs puros)
│   └── Entities/                        # 14 entidades
│
├── Clinical.Application.DTOS/           # Data Transfer Objects
│   └── (una carpeta por módulo)/        # DTOs de request y response
│
├── Clinical.Interface/                  # Abstracciones de repositorio
│   └── Interfaces/
│       ├── IGenericRepository<T>        # GetAll/GetById/Exec/GetAllAsync(params)
│       ├── IUnitOfWork                  # Agrega todos los repos genéricos
│       ├── IAppointmentRepository       # Queries con JOIN → retorna DTOs
│       ├── IPrescriptionRepository      # Prescription + líneas de detalle
│       └── IAuthRepository             # Lookup de usuario + refresh token
│
├── Clinical.Persistence/                # Acceso a datos — Dapper + SQL Server
│   ├── Context/ApplicationDBContext.cs
│   ├── Repositories/                    # 7 implementaciones
│   └── Extensions/InyectionExtensions.cs
│
├── Clinical.Infraestructure/            # Infraestructura transversal
│   └── Services/JwtTokenService.cs      # Generación de access token + refresh token
│
├── Clinical.Utils/                      # Constantes y helpers compartidos
│   └── Constants/
│       ├── GlobalMessage.cs             # Mensajes de respuesta (español)
│       └── StoreProcedures.cs           # Nombres de todos los SPs (~130 entradas)
│
├── Clinical.Tests/                      # Pruebas unitarias — xUnit + Moq
│
├── Database/
│   ├── Scripts_StoredProcedures.sql     # DDL módulos base + stored procedures
│   └── Scripts_NewModules.sql           # DDL módulos nuevos + stored procedures + seed
│
├── docker-compose.yml                   # API + Web + SQL Server 2022
├── ClinicalAPI.sln
└── .github/workflows/dotnet-desktop.yml # CI/CD — build + test en .NET 10
```

---

## Licencia

Ver [LICENSE.txt](LICENSE.txt).
