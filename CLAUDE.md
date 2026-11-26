# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Qué es

API REST (.NET 10, ASP.NET Core) para gestionar un refugio de animales: ingreso, alojamiento, alimentación, salud y adopción. Proyecto de Taller II. El código, los nombres de clases y los mensajes están en español.

## Comandos

```bash
dotnet build PatitasCalchaquies.slnx
dotnet run --project PatitasCalchaquies.API          # Swagger/OpenAPI solo en Development
dotnet test                                          # un solo test: dotnet test --filter "FullyQualifiedName~ProbarConexionAMySql"
node generar-bruno.js                                # regenera requests en ./bruno-collection (también generar-bruno-faltantes.js)
```

No hay linter configurado. El único test (`PatitasCalchaquies.Tests/TestConexion.cs`) es de integración: necesita un MySQL local real y trae la cadena de conexión escrita en el código.

## Configuración

- `appsettings.Development.json` está en `.gitignore` y es donde viven la cadena de conexión (`ConnectionStrings:DefaultConnection`) y `Jwt:llave_secreta`. El `appsettings.json` versionado los tiene vacíos; sin el archivo de Development la API no arranca.
- Esquema de la base de datos: `PatitasCalchaquies.Infrastructure/DataBase/PatitasCalchaquies_completa.sql` (tablas y todos los stored procedures).
- `AdminInicial:Usuario` / `AdminInicial:Password` (opcionales): al arrancar, `API/Seed/SemillaAdministrador.cs` crea ese usuario con rol Administrador si no existe. Sin password no hace nada.

## Arquitectura

Capas en cuatro proyectos; la dependencia va API → Core ← Infrastructure.

- **Core**: entidades (`Entities`), interfaces de repositorio (`Interfaces`) y servicios con las reglas de negocio y validaciones básicas (`Services`, clases `Servicio*`). No conoce la base de datos.
- **Infrastructure**: repositorios (`Repositorios*`) que acceden a MySQL con **Dapper y stored procedures** (no hay Entity Framework). Cada método llama a un `sp_<entidad>_<accion>` con `CommandType.StoredProcedure`, atrapa excepciones y las registra con `ArchivoLog.RegistrarErrores`, devolviendo vacío/`0`/`false` en vez de propagar. `Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true` mapea columnas con guion bajo.
- **API**: controladores, DTOs (`...CrearDto`, `...ActualizarDto`, `...Dto`), validadores FluentValidation y el perfil de AutoMapper (`AutoMapper/MappeoObjeto.cs`). Los controladores no exponen entidades directamente.
- `Shared` está vacío.

### Cablear una entidad nueva

Hay que tocar varios sitios; olvidar uno falla en runtime, no al compilar:
1. Entidad en `Core/Entities` con atributos `[Table]/[Column]`; interfaz en `Core/Interfaces`; `Servicio*` en `Core/Services`.
2. Repositorio en `Infrastructure/Repositories` más sus stored procedures en el `.sql`.
3. Registrar repositorio **y** servicio en `Infrastructure/Extensions/ServiceExtensions.cs` (`AddInfrastructure`).
4. DTOs, mapeos en `MappeoObjeto.cs`, controlador y los validadores crear/actualizar, registrados a mano en `API/Program.cs` (`AddScoped<IValidator<...>>`).
5. Agregar el recurso a `generar-bruno.js` si se quiere la colección de Bruno.

Los nombres de interfaz no son uniformes (`IRepositoryX` vs `IRepositorioX`); revisar el existente antes de crear uno.

## Autenticación

- Login: `POST /api/Auth/login` → token JWT (HS256, 180 min por defecto), fecha `Expiracion` y datos del `Operador`. `AutenticacionService` valida con BCrypt (`ServicioPassword`); si el usuario no tiene roles se le asigna `Voluntario`.
- Los permisos siguen el diagrama de casos de uso con tres políticas definidas en `Program.cs` (nombres en `Shared/Constants/Roles.cs`): `Politicas.Voluntario` (cualquiera de los tres roles: Administrador y Veterinario especializan a Voluntario), `Politicas.Administrador` y `Politicas.Veterinario`. El Administrador **no** hereda los casos del Veterinario.
- Todos los controladores llevan `[Authorize(Policy = Politicas.Voluntario)]` a nivel de clase, salvo `AuthController`, `WeatherForecastController`, `UsuariosController` y `UsuariosRolesController` (estos dos llevan `Politicas.Administrador`).
- Los GET quedan abiertos a cualquier rol. Las escrituras (POST/PUT/DELETE) llevan además `[Authorize(Policy = ...)]` en el método: `Politicas.Administrador` en catálogos (Especies, Raza, Sexos, EstadosAnimales, Vacuna, Medicamento), Predios, Sectores, Veterinarios y Roles; `Politicas.Veterinario` en AtencionesVeterinarias, Tratamientos, TratamientosMedicamentos, Vacunaciones y Esterilizaciones.
- `UsuariosController` mantiene además `VerificarPermisoAdministradorAsync` en los endpoints de `operadores` y `POST /api/Usuarios`.
- Un controlador nuevo debe llevar la política que le corresponda según el diagrama, con `using Microsoft.AspNetCore.Authorization;` y `using PatitasCalchaquies.Shared.Constants;`.

## Bruno

`bruno-collection/` tiene una carpeta por recurso con `auth: inherit`. Con `[Authorize]` activo, las requests necesitan el Bearer token obtenido en el login. Los scripts `generar-bruno*.js` generan YAML de Bruno 4.x; las rutas son `api/<NombreDelControlador>` (por ejemplo `api/Animales`, `api/Raza`).
