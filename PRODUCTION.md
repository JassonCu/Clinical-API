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

## Pendientes antes de producción (ver revisión completa)

Esto cubre solo **secretos y configuración (P0-1/P0-2)**. Antes de lanzar quedan:
hashear refresh tokens, `UseForwardedHeaders` para el rate limiting, actualizar AutoMapper (vuln. HIGH),
health check con chequeo de BD, deploy repetible del esquema/SPs, y auditoría/cifrado de datos clínicos.
