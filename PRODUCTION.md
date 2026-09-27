# Configuración de Producción

Este proyecto **no lleva secretos en archivos versionados**. En producción, la clave de
firma JWT, la cadena de conexión y los orígenes CORS se inyectan por **variables de entorno**.
La app **no arranca** si falta la clave o la cadena de conexión (validación _fail-fast_ en `Program.cs`).

> ⚠️ **Antes de lanzar:** la `SecretKey` de ejemplo (`...ChangeInProduction...`) quedó en el
> historial de git. **Rotá la clave** (generá una nueva, abajo) y **purgá el historial**
> (`git filter-repo` / BFG) antes de exponer el repo. Una clave filtrada permite **firmar tokens de Admin**.

## Cómo se resuelve la configuración

`appsettings.json` (base, sin secretos) → `appsettings.{Environment}.json` → **variables de entorno** (ganan).
En .NET, un `:` en la clave se escribe `__` (doble guion bajo) en la variable de entorno.

## Variables de entorno requeridas — API (`Clinical.API`)

| Variable | Ejemplo | Notas |
|----------|---------|-------|
| `ASPNETCORE_ENVIRONMENT` | `Production` | Activa HSTS, HTTPS redirect y logging Warning |
| `JwtSettings__SecretKey` | *(clave nueva, ≥32 chars)* | **Secreto.** No puede contener `ChangeInProduction` ni empezar con `DEV-ONLY` |
| `ConnectionStrings__ClinicalConnection` | `Server=...;Database=Clinical;User Id=...;Password=...;Encrypt=true` | **Secreto** |
| `Cors__AllowedOrigins__0` | `https://app.tudominio.com` | Origen real del frontend (agregá `__1`, `__2`, … si hay más) |

Opcionales (tienen default en `appsettings.json`): `JwtSettings__Issuer`, `JwtSettings__Audience`,
`JwtSettings__AccessTokenExpiryMinutes`, `JwtSettings__RefreshTokenExpiryDays`.

## Variables de entorno requeridas — Web (`Clinical.Web`)

| Variable | Ejemplo | Notas |
|----------|---------|-------|
| `ASPNETCORE_ENVIRONMENT` | `Production` | |
| `ApiSettings__BaseUrl` | `https://api.tudominio.com` | URL pública de la API |

## Logging

- **Consola**: JSON compacto (CompactJson) en Producción — pensado para que lo recolecte el orquestador/contenedor. En Development es legible para humanos.
- **Archivo**: `logs/clinical-*.log` (API) y `logs/clinical-web-*.log` (Web), JSON, rotación diaria, **máx. 50 MB por archivo y 30 archivos retenidos** (no llena disco). En contenedores es efímero; la fuente de verdad debe ser la consola/Seq.
- **Enriquecido** con `MachineName`, `EnvironmentName`, `ProcessId`, `ThreadId`, `Application`, `TraceId`, y por request: `RequestHost`, `ClientIp`, `UserAgent`, `StatusCode`, `UserId`/`UserName` (si está autenticado). **No se registran cuerpos ni cabeceras** (nada de contraseñas/PHI).
- **Niveles por request**: 5xx/excepción → Error, 4xx → Warning, `/health` y estáticos → Verbose, resto → Information.

**Logging centralizado (opcional, Seq)** — se activa solo si configurás la URL:

| Variable | Ejemplo |
|----------|---------|
| `Serilog__SeqUrl` | `https://seq.tudominio.com` |
| `Serilog__SeqApiKey` | *(si tu Seq lo requiere)* |

Sin `Serilog__SeqUrl`, el sink de Seq queda desactivado. Para otro backend (Elastic, App Insights, Loki, etc.) alcanza con agregar el sink correspondiente en `Program.cs`.

## Bloqueo de cuenta (login)

Tras varios intentos fallidos, el usuario se bloquea temporalmente (complementa el rate limiting por IP). Ajustable por variables de entorno (defaults entre paréntesis):

| Variable | Default |
|----------|---------|
| `Auth__Lockout__MaxFailedAttempts` | 5 |
| `Auth__Lockout__WindowMinutes` | 15 |
| `Auth__Lockout__LockoutMinutes` | 15 |

> El conteo es **en memoria por instancia**. Para un despliegue multi-instancia, respaldalo con una caché distribuida (Redis). Nota: un bloqueo por usuario permite un DoS dirigido (bloquear a un usuario legítimo a propósito); por eso la ventana es corta y configurable.

## Restablecimiento de contraseña

Hoy el reset es **mediado por admin**: un admin genera el token (`POST /api/user/{id}/reset-password`) y lo comparte con el usuario. Por defecto, sin canal de entrega configurado, el token vuelve **en el body** (con `Cache-Control: no-store`) para que el admin lo comparta manualmente. En la BD solo se guarda el **hash**.

Para **dejar de exponer el token** en producción, implementá `IPasswordResetNotifier` (p. ej. envío por email) devolviendo `true`, y registralo en `AddInyectionInfrastructure` en lugar de `NullPasswordResetNotifier`. Cuando la entrega es directa, la API ya **no incluye** el token en la respuesta.

## Generar una `SecretKey` fuerte

```bash
openssl rand -base64 48
```
```powershell
# PowerShell
[Convert]::ToBase64String((1..48 | ForEach-Object { Get-Random -Max 256 }))
```

## Ejemplo (Linux / contenedor)

```bash
export ASPNETCORE_ENVIRONMENT=Production
export JwtSettings__SecretKey='<clave-generada>'
export ConnectionStrings__ClinicalConnection='Server=...;Database=Clinical;User Id=...;Password=...;Encrypt=true'
export Cors__AllowedOrigins__0='https://app.tudominio.com'
dotnet Clinical.API.dll
```

## Desarrollo local

No requiere configuración: `appsettings.Development.json` trae una clave de dev y la cadena de
conexión local. Nunca uses esos valores fuera de tu máquina.

## Auditoría clínica

Cada operación que **modifica** datos (comandos) genera una entrada de auditoría automática vía el
pipeline CQRS (`AuditBehaviour`): **qué acción, sobre qué registro, quién, desde qué IP, cuándo y si tuvo éxito**.
Las lecturas (queries) no se auditan. No se registran cuerpos ni datos sensibles.

Las entradas usan el `SourceContext` **`Audit`** y se emiten siempre (un override de nivel evita que el
umbral de producción las filtre). Para conservarlas de forma separada, enrutá ese contexto a su propio
sink en `Program.cs`, por ejemplo:

```csharp
loggerConfiguration.WriteTo.Logger(lc => lc
    .Filter.ByIncludingOnly(e => e.Properties.ContainsKey("SourceContext")
        && e.Properties["SourceContext"].ToString().Contains("Audit"))
    .WriteTo.File(new CompactJsonFormatter(), "logs/audit-.log",
        rollingInterval: RollingInterval.Day, retainedFileCountLimit: 3650));
```

> El destino por defecto es el log estructurado (consola/archivo/Seq). Si tu normativa exige un
> registro **inmutable / a prueba de manipulación**, reemplazá `IAuditLogger` por una implementación
> que escriba a un almacén append-only (tabla WORM, servicio de auditoría dedicado, etc.).

## Cifrado en reposo

- **En reposo (base de datos):** activá **TDE** con [`Database/Scripts_Encryption_TDE.sql`](Database/Scripts_Encryption_TDE.sql)
  (lo aplica un DBA; cifra datos, log y backups sin cambios de código). **Guardá el certificado + su clave
  privada fuera del server**: sin ellos no se restaura un backup cifrado.
- **En tránsito:** usá `Encrypt=true` en la cadena de conexión (`ConnectionStrings__ClinicalConnection`)
  y TLS en el borde (HTTPS).

## Pendientes antes de producción

**Ya resuelto en código** (referencia): secretos fuera de git + binding por env con fail-fast, IDOR de
change-password, hasheo de refresh tokens, mitigación de enumeración por timing + account lockout,
ProblemDetails, headers de seguridad + CSP, `UseForwardedHeaders` (restringido por proxies configurados),
AutoMapper actualizado, health check con chequeo de BD, auditoría clínica (`AuditBehaviour`), logging
estructurado, y `no-store` en el reset. CI + CodeQL + Dependabot activos.

**Acciones tuyas antes de lanzar (infra/operación):**
1. **Rotar la `SecretKey`** y **purgar el historial de git** (la clave de ejemplo quedó en commits previos).
2. Setear las variables de entorno de producción (esta guía), incluida `ForwardedHeaders__KnownProxies__0`
   con la IP de tu proxy/balanceador.
3. **TDE** para cifrado en reposo (`Database/Scripts_Encryption_TDE.sql`) + `Encrypt=true` en la conexión.
4. **Auditoría inmutable** si tu normativa lo exige (implementar `IAuditLogger` contra un store append-only).
5. **Entrega de reset por email** (implementar `IPasswordResetNotifier`) para dejar de devolver el token en el body.
6. Deploy repetible del esquema/SPs, backups, monitoreo, y **branch protection** en GitHub (exigir que el CI pase).
