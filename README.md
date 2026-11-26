# Patitas Calchaquíes · API

API REST para la gestión de un refugio de animales: ingreso, salud, alojamiento, alimentación y adopción.
ASP.NET Core (.NET 10), MySQL con Dapper y stored procedures, autenticación con JWT.

## Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- MySQL **8.0 o superior** (el script usa el collation `utf8mb4_0900_ai_ci`, que MariaDB no soporta)
- [Bruno](https://www.usebruno.com/) para probar los endpoints (opcional)

## 1. Crear la base de datos

El script crea la base `PatitasCalchaquies` con sus tablas, procedimientos almacenados y datos iniciales (catálogos, roles y usuarios de prueba). Si la base ya existe, recrea las tablas.

```bash
mysql -u root -p < PatitasCalchaquies.Infrastructure/DataBase/PatitasCalchaquies_completa.sql
```

## 2. Configurar la API

Los secretos no están en el repositorio. Crear el archivo `PatitasCalchaquies.API/appsettings.Development.json` (ya está en el `.gitignore`) con este contenido, completando la contraseña de MySQL:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=127.0.0.1;Port=3306;Database=PatitasCalchaquies;User Id=root;Password=TU_PASSWORD;"
  },
  "Jwt": {
    "llave_secreta": "una-llave-de-al-menos-32-caracteres-para-hs256"
  }
}
```

- `Jwt:llave_secreta` debe tener **al menos 32 caracteres**: HS256 necesita una clave de 256 bits.
- Opcional: `"AdminInicial": { "Usuario": "...", "Password": "..." }` crea al iniciar un usuario Administrador adicional si no existe.

## 3. Ejecutar

```bash
dotnet run --project PatitasCalchaquies.API
```

La API queda en `http://localhost:5227`. En entorno Development, la especificación OpenAPI está en `http://localhost:5227/openapi/v1.json`.

## 4. Usuarios de prueba

Los crea el script SQL. Las contraseñas se guardan hasheadas con BCrypt.

| Usuario | Contraseña | Rol |
|---|---|---|
| `admin.valles` | `calchaqui2026` | Administrador |
| `dra.mamani` | `vet2026` | Veterinario |
| `dr.tolaba` | `vet2026` | Veterinario |
| `voluntario.cafayate` | `patitas2026` | Voluntario |

## 5. Probar con Bruno

Todos los endpoints, salvo el login, requieren un token JWT.

1. Abrir la carpeta `bruno-collection/` en Bruno y seleccionar el environment `ji` (define `baseurl`).
2. Ejecutar `Animales > Get All` sin token: responde **401 Unauthorized**.
3. Ejecutar `Auth > Login` con uno de los usuarios de prueba:
   ```json
   { "usuario": "admin.valles", "password": "calchaqui2026" }
   ```
   La respuesta trae `token`, `expiracion` y `operador`.
4. Guardar el token en una variable `token` del environment. Para hacerlo automáticamente, agregar en la pestaña *Script → Post Response* del login:
   ```js
   bru.setEnvVar("token", res.body.token);
   ```
5. En la configuración de la colección, pestaña **Auth**, elegir **Bearer Token** con el valor `{{token}}`. Todas las requests lo heredan.
6. Repetir `Animales > Get All`: responde **200**.

Para ver la diferencia entre 401 y 403: con el token de `voluntario.cafayate`, `POST /api/Usuarios/operadores` responde **403 Forbidden**, porque ese endpoint exige rol Administrador.

## Estructura

| Proyecto | Responsabilidad |
|---|---|
| `PatitasCalchaquies.API` | Controllers, DTOs, validadores (FluentValidation), mapeo (AutoMapper), configuración |
| `PatitasCalchaquies.Core` | Entidades, interfaces de repositorio y servicios con las reglas de negocio |
| `PatitasCalchaquies.Infrastructure` | Repositorios con Dapper, conexión a MySQL y script de la base |
| `PatitasCalchaquies.Tests` | Pruebas con xUnit |
