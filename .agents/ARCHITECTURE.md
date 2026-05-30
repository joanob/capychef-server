# Arquitectura del proyecto Capychef Server

## Visión general

API REST en .NET 8 con dos proyectos principales:

- **`Capychef/`** — lógica de negocio, dominio, repositorios y servicios (sin dependencias de ASP.NET)
- **`Capychef.Api/`** — capa HTTP API REST: controllers, middlewares, autenticación, configuración

Base de datos: PostgreSQL. Migraciones con Flyway (carpeta `Migrations/`). ORM: Entity Framework Core.
Tiempo real: SignalR (`/realtime`). Cache: Redis. Envío de emails: SMTP (Smtp4Dev en DEV).

---

## Estructura de `Capychef/` (lógica de negocio)

Cada módulo de negocio sigue exactamente esta estructura interna:

```
NombreModulo/
├── Domain/
│   ├── Cmd/          → comandos de escritura (input). Implementan ICmd con método Validate()
│   ├── DTO/          → objetos de salida hacia la API
│   ├── Entities/     → entidades EF Core mapeadas a tablas
│   ├── Errors/       → errores específicos del módulo (heredan AppError)
│   └── Interfaces/   → interfaces de servicios y repositorios
├── Repositories/     → implementaciones de repositorios (acceso a DB)
└── Services/         → implementaciones de servicios (lógica de negocio)
```

### Módulos de negocio

| Módulo | Descripción |
|--------|-------------|
| `Users/` | Usuarios, sesiones, contraseñas, tokens de email/recovery. Tres tipos de usuario: guest, password, email |
| `Households/` | Grupos domésticos. Incluye membresías, invitaciones, join requests y espacios de almacenamiento |
| `Food/` | Catálogo de alimentos, categorías, unidades de medida (UoM) y su historial de modificaciones |
| `Storage/` | Lotes (batches) de alimentos almacenados en un household |
| `Shopping/` | Listas de la compra, supermercados y detalles de precio por supermercado |
| `Recipes/` | Recetas del household |
| `Gourmet/` | Módulo de suscripción premium |
| `DataLoader/` | Carga de datos iniciales desde JSON (`CapychefData/CapychefData.json`) |
| `Testdata/` | Datos de prueba para tests E2E |

### Carpetas transversales en `Capychef/`

```
Common/
├── Auth/        → AuthUserDetails (datos del usuario autenticado en memoria)
├── Entities/    → EntityType (enum de tipos de entidad para errores)
├── Errors/      → AppError, ErrorType, NotFoundError, ValidationError
├── Interfaces/  → ICmd
├── Result/      → Result<T> (wrapper de resultado/error)
└── Utils/       → utilidades generales

Infrastructure/
├── Interfaces/  → IEmailSender
└── DevImplementations/ → Smtp4DevSender (implementación para desarrollo)

Persistence/
└── CapychefDbContext.cs  → DbContext de EF Core con todos los DbSets
```

---

## Estructura de `Capychef.Api/` (capa HTTP)

```
Capychef.Api/
├── Program.cs                  → entrada, pipeline HTTP, configuración de servicios
├── AppEnvironment.cs           → constantes DEV/QA/PROD + extensiones IsDev(), IsQa(), IsProd()
├── CorsSetup.cs                → configuración CORS desde CORS_ALLOWED_ORIGINS env var
├── Controllers/                → un controller por módulo
├── Auth/                       → JWT, middleware de autenticación, rate limiting
├── Authorization/              → filtros CheckMembership y CheckOwnership
├── Errors/                     → ApiResponse<T>, ApiError, GlobalErrorHandler
├── Logging/                    → SerilogSetup
└── Realtime/                   → SignalR hub y servicios de conexión
```

### Controllers

Un controller por módulo de negocio. Nomenclatura: `{Modulo}Controller.cs`.
Inyectan el/los servicios del módulo vía constructor (primary constructor de C#).

### Auth (`Capychef.Api/Auth/`)

| Archivo | Responsabilidad |
|---------|----------------|
| `AuthMiddleware.cs` | Valida JWT en cada request. Rutas públicas definidas en `_publicRoutes[]` |
| `JwtService.cs` | Crea y valida JWT. Dos cookies: `CAPYCHEF_AUTH_SESSION` (5 min) y `CAPYCHEF_AUTH_REFRESH` (100 años) |
| `AuthUserDetailsService.cs` | Almacena/recupera `AuthUserDetails` en `HttpContext.Items` |
| `RateLimiterSetup.cs` | En DEV: no-op. En QA/PROD: Redis con políticas por IP |
| `RateLimiterPolicies.cs` | Constantes de nombres de políticas |
| `ILoginAttemptTracker.cs` | Interfaz para tracking de intentos fallidos de login |
| `RedisLoginAttemptTracker.cs` | Implementación Redis (QA/PROD) |
| `NoOpLoginAttemptTracker.cs` | Implementación vacía (DEV) |

### Authorization (`Capychef.Api/Authorization/`)

- `[CheckMembership]` — verifica que el usuario pertenece al household activo
- `[CheckOwnership]` — verifica que el usuario es owner del household activo

El household activo viaja en el JWT (claim `householdId`).

---

## Patrones y convenciones

### Comandos (Cmd)
Los inputs de escritura se modelan como clases `*Cmd` que implementan `ICmd`:
```csharp
public class CreateXCmd : ICmd {
    public ValidationError? Validate() { ... }
}
```
El servicio llama a `cmd.Validate()` antes de procesar.

### Result<T>
Los servicios devuelven `Result<T>` en lugar de lanzar excepciones:
```csharp
if (result.Failed()) return HandleError(result.Error(), logger);
var data = result.Get();
```
Para operaciones que solo devuelven error o null: `AppError?`.

### Errores
- Errores genéricos: `Common/Errors/` (`NotFoundError`, `ValidationError`)
- Errores específicos de módulo: `NombreModulo/Domain/Errors/`
- Todos heredan `AppError` que tiene `ErrorType` y `EntityDetails`
- `GlobalErrorHandler` mapea `ErrorType` a HTTP status codes

### Respuesta HTTP
```csharp
// Éxito
return new ApiResponse<UserDto>(user);
// Error
return new ApiResponse<UserDto>(new ApiError(error, "CODIGO_ERROR"));
```

### Repositorios
- Los métodos `GetTracked*` devuelven entidades con tracking de EF (para modificar y guardar)
- Los métodos `Get*` sin Tracked devuelven `AsNoTracking()` (solo lectura)
- Todos los métodos de lectura usan `.Active()` extension que filtra `!IsDeleted && !IsBlocked`

### Entidades
- Columnas mapeadas con `[Column("nombre_columna")]`
- Constructores protegidos para EF + constructores públicos para creación
- Factory methods estáticos para creación: `User.NewEmailUser(...)`, `User.NewGuestUser(...)`

### DI
Todo registrado en `Capychef/DependencyInjection.cs` como `AddScoped`.
Los servicios de la API (rate limiting, CORS) se registran en `Program.cs` o sus propias clases Setup.

---

## Entornos

| Variable | DEV | QA | PROD |
|----------|-----|----|------|
| `ASPNETCORE_ENVIRONMENT` | `DEV` | `QA` | `PROD` |
| Rate limiting | ❌ no-op | ✅ Redis | ✅ Redis |
| Secure cookies | ❌ | ✅ | ✅ |
| REDIS_CONNECTION_STRING | no requerido | requerido | requerido |
| HTTPS redirect | ❌ | ✅ | ✅ |
| Swagger | ✅ | ❌ | ❌ |

Archivos de entorno: `.env.dev`, `.env.qa`, `.env.prod`. El activo siempre es `.env`.

---

## Migraciones

Flyway con SQL puro. Carpeta `Migrations/migrations/`. Nomenclatura: `V{numero}__NombreModulo.sql`.
El orden de migraciones refleja dependencias entre módulos (Users → Households → Food → ...).
