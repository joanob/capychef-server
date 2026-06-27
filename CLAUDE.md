# CLAUDE.md — Capychef Server

Lee siempre `.agents/ARCHITECTURE.md` y `.agents/SECURITY_IGNORE.md` antes de generar código.

Ahorra tokens al máximo, no hace falta que me expliques cosas innecesarias por el chat ni crees documentación, solamente crea el código.

## Stack

- **Lenguaje / Framework:** C# 12 / ASP.NET Core 8 (.NET 8)
- **Base de datos:** PostgreSQL 16 con Flyway (migraciones SQL puras)
- **ORM:** Entity Framework Core 8
- **Caché / Rate limiting:** Redis 7
- **Tiempo real:** SignalR en `/realtime`
- **Email:** SMTP (Smtp4Dev en DEV)
- **Auth:** JWT en cookies (`CAPYCHEF_AUTH_SESSION` 5 min, `CAPYCHEF_AUTH_REFRESH` 100 años)
- **Logging:** Serilog
- **Tests:** NUnit + Dapper (E2E únicamente)

## Estructura de proyectos

| Proyecto | Rol |
|----------|-----|
| `Capychef/` | Dominio, repositorios, servicios — sin dependencias ASP.NET |
| `Capychef.Api/` | Controllers, middlewares, auth, configuración HTTP |
| `Capychef.E2E/` | Tests end-to-end |
| `Migrations/` | SQL Flyway (`V{n}__{Modulo}.sql`) |

## Módulos de negocio

`Users`, `Households`, `Food`, `Storage`, `Shopping`, `Recipes`, `Gourmet`, `DataLoader`, `Testdata`

Cada módulo: `Domain/{Cmd,DTO,Entities,Errors,Interfaces}` + `Repositories/` + `Services/`

## Patrones clave

- **Comandos:** clases `*Cmd` que implementan `ICmd` con `Validate()` — el servicio lo llama antes de procesar
- **Result<T>:** los servicios devuelven `Result<T>` o `AppError?`, nunca lanzan excepciones
- **Errores:** heredan `AppError`; genéricos en `Common/Errors/`, específicos en `Modulo/Domain/Errors/`
- **Respuesta HTTP:** `ApiResponse<T>` con `ApiError` — `GlobalErrorHandler` mapea `ErrorType` a HTTP status
- **Repositorios:** `GetTracked*` con tracking EF (escritura), `Get*` con `AsNoTracking()` (lectura); todos usan `.Active()` (filtra `IsDeleted` e `IsBlocked`)
- **Entidades:** constructores protegidos para EF, factory methods estáticos para creación
- **DI:** todo `AddScoped` registrado en `Capychef/DependencyInjection.cs`
- **Tiempos:** siempre UTC

## Entornos

Archivos: `.env.dev`, `.env.qa`, `.env.prod`. El activo siempre es `.env`.

| | DEV | QA | PROD |
|-|-----|----|------|
| Rate limiting | ❌ | ✅ Redis | ✅ Redis |
| Secure cookies | ❌ | ✅ | ✅ |
| Swagger | ✅ | ❌ | ❌ |

## Reglas para el agente

- No ejecutar builds para comprobar errores — el desarrollador los lanza manualmente
- No generar comentarios ni XML docs en el código generado
- No generar documentación sin petición explícita
- El desarrollador usa archivos `.log` como interfaz de tareas y análisis
- Si hay dudas, añadirlas al final del archivo de tareas o en el chat
- Escribir código limpio con separación de responsabilidades
