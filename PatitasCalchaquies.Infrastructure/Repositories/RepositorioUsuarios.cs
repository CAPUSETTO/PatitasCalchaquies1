using System.Data;
using Dapper;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Core.Services;
using PatitasCalchaquies.Infrastructure.Context;


namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioUsuario : IRepositorioUsuario
{
    private readonly IMyConexion _conn;
    private static bool _esquemaVerificado;
    private static readonly SemaphoreSlim _lock = new(1, 1);

    public RepositorioUsuario(IMyConexion conn)
    {
        _conn = conn;
    }

    private static async Task AsegurarEsquemaYCatalogosOperadoresAsync(IDbConnection conexion)
    {
        if (_esquemaVerificado) return;

        await _lock.WaitAsync();
        try
        {
            if (_esquemaVerificado) return;

            // 1. Asegurar columnas extendidas en la tabla Usuarios para almacenar identidad y vínculo con Veterinario
            var columnasExistentes = (await conexion.QueryAsync<string>(@"
                SELECT LOWER(COLUMN_NAME)
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_SCHEMA = DATABASE() AND LOWER(TABLE_NAME) = 'usuarios';
            ")).ToHashSet();

            if (!columnasExistentes.Contains("nombre"))
            {
                await conexion.ExecuteAsync("ALTER TABLE `Usuarios` ADD COLUMN `nombre` VARCHAR(50) DEFAULT NULL;");
            }
            if (!columnasExistentes.Contains("apellido"))
            {
                await conexion.ExecuteAsync("ALTER TABLE `Usuarios` ADD COLUMN `apellido` VARCHAR(50) DEFAULT NULL;");
            }
            if (!columnasExistentes.Contains("etiqueta_area"))
            {
                await conexion.ExecuteAsync("ALTER TABLE `Usuarios` ADD COLUMN `etiqueta_area` VARCHAR(100) DEFAULT NULL;");
            }
            if (!columnasExistentes.Contains("id_veterinario"))
            {
                await conexion.ExecuteAsync("ALTER TABLE `Usuarios` ADD COLUMN `id_veterinario` INT DEFAULT NULL;");
            }

            // 2. Asegurar los 3 roles base del sistema
            await conexion.ExecuteAsync(@"
                INSERT IGNORE INTO `Roles` (`IdRol`, `nombre`, `descripcion`) VALUES
                (1, 'Administrador', 'Coordinación General · Control total de ingresos, adopciones, clínica y alta de personal'),
                (2, 'Veterinario', 'Veterinaria Clínica · Gestión de historial clínico, vacunaciones, cirugías y vademécum'),
                (3, 'Voluntario', 'Guardia y Adopciones · Seguimiento de solicitudes de adopción, entrevistas y fichas');
            ");

            // 3. Asegurar los veterinarios base del refugio
            await conexion.ExecuteAsync(@"
                INSERT IGNORE INTO `Veterinarios` (`IdVeterinario`, `nombre`, `apellido`, `telefono`, `matricula`) VALUES
                (1, 'Sofía', 'Mamaní', '+54 3868 421900', 'MP-SAL-0418'),
                (2, 'Lucas', 'Tolaba', '+54 3868 438810', 'MP-SAL-0291');
            ");

            // 4. Si la tabla Usuarios está vacía, sembrar los 4 operadores iniciales del refugio con BCrypt
            var totalUsuarios = await conexion.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM `Usuarios`;");
            if (totalUsuarios == 0)
            {
                var hashAdmin = ServicioPassword.HashPassword("calchaqui2026");
                var hashVet = ServicioPassword.HashPassword("vet2026");
                var hashVol = ServicioPassword.HashPassword("patitas2026");

                await conexion.ExecuteAsync(@"
                    INSERT IGNORE INTO `Usuarios` (`IdUsuario`, `usuario`, `password_hash`, `activo`, `fecha_creacion`, `ultimo_acceso`, `nombre`, `apellido`, `etiqueta_area`, `id_veterinario`)
                    VALUES
                    (1, 'admin.valles', @hashAdmin, 1, '2025-03-10', NOW(), 'Coordinación General', 'Valles', 'Coordinación General', NULL),
                    (2, 'dra.mamani', @hashVet, 1, '2025-05-14', NOW(), 'Sofía', 'Mamaní', 'Veterinaria Clínica', 1),
                    (3, 'voluntario.cafayate', @hashVol, 1, '2025-08-01', NOW(), 'Guardia Voluntaria', 'Cafayate', 'Guardia y Adopciones', NULL),
                    (4, 'dr.tolaba', @hashVet, 1, '2025-09-20', NULL, 'Lucas', 'Tolaba', 'Cirugía y Esterilizaciones', 2);

                    INSERT IGNORE INTO `Usuario_Rol` (`IdUsuario`, `IdRol`) VALUES
                    (1, 1),
                    (2, 2),
                    (3, 3),
                    (4, 2);
                ", new { hashAdmin, hashVet, hashVol });
            }

            _esquemaVerificado = true;
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            await AsegurarEsquemaYCatalogosOperadoresAsync(conexion);
            return await conexion.QueryAsync<Usuario>(
                "sp_usuario_obtener_todos",
                commandType: CommandType.StoredProcedure
            );
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<Usuario>();
        }
    }

    public async Task<IEnumerable<Usuario>> ObtenerOperadoresDetalladosAsync()
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            await AsegurarEsquemaYCatalogosOperadoresAsync(conexion);

            const string sql = @"
                SELECT
                    u.IdUsuario,
                    u.usuario AS User,
                    u.password_hash AS PasswordHash,
                    u.activo AS Activo,
                    u.fecha_creacion AS FechaCreacion,
                    u.ultimo_acceso AS UltimoAcceso,
                    COALESCE(u.nombre, v.nombre) AS Nombre,
                    COALESCE(u.apellido, v.apellido) AS Apellido,
                    u.etiqueta_area AS EtiquetaArea,
                    COALESCE(u.id_veterinario, v.IdVeterinario) AS IdVeterinario,
                    COALESCE(r.IdRol, 3) AS IdRol,
                    COALESCE(r.nombre, 'Voluntario') AS RolNombre,
                    r.descripcion AS RolDescripcion,
                    v.matricula AS VeterinarioMatricula,
                    v.nombre AS VeterinarioNombre,
                    v.apellido AS VeterinarioApellido,
                    v.telefono AS VeterinarioTelefono
                FROM `Usuarios` u
                LEFT JOIN `Usuario_Rol` ur ON ur.IdUsuario = u.IdUsuario
                LEFT JOIN `Roles` r ON r.IdRol = ur.IdRol
                LEFT JOIN `Veterinarios` v ON v.IdVeterinario = u.id_veterinario
                ORDER BY u.IdUsuario ASC;
            ";

            return await conexion.QueryAsync<Usuario>(sql);
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<Usuario>();
        }
    }

    public async Task<Usuario?> ObtenerOperadorDetalladoPorIdAsync(int idUsuario)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            await AsegurarEsquemaYCatalogosOperadoresAsync(conexion);

            const string sql = @"
                SELECT
                    u.IdUsuario,
                    u.usuario AS User,
                    u.password_hash AS PasswordHash,
                    u.activo AS Activo,
                    u.fecha_creacion AS FechaCreacion,
                    u.ultimo_acceso AS UltimoAcceso,
                    COALESCE(u.nombre, v.nombre) AS Nombre,
                    COALESCE(u.apellido, v.apellido) AS Apellido,
                    u.etiqueta_area AS EtiquetaArea,
                    COALESCE(u.id_veterinario, v.IdVeterinario) AS IdVeterinario,
                    COALESCE(r.IdRol, 3) AS IdRol,
                    COALESCE(r.nombre, 'Voluntario') AS RolNombre,
                    r.descripcion AS RolDescripcion,
                    v.matricula AS VeterinarioMatricula,
                    v.nombre AS VeterinarioNombre,
                    v.apellido AS VeterinarioApellido,
                    v.telefono AS VeterinarioTelefono
                FROM `Usuarios` u
                LEFT JOIN `Usuario_Rol` ur ON ur.IdUsuario = u.IdUsuario
                LEFT JOIN `Roles` r ON r.IdRol = ur.IdRol
                LEFT JOIN `Veterinarios` v ON v.IdVeterinario = u.id_veterinario
                WHERE u.IdUsuario = @idUsuario
                LIMIT 1;
            ";

            return await conexion.QueryFirstOrDefaultAsync<Usuario>(sql, new { idUsuario });
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return null;
        }
    }
    
    public async Task<Usuario?> ObtenerPorUsuarioAsync(string usuario)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            await AsegurarEsquemaYCatalogosOperadoresAsync(conexion);

            var parametros = new DynamicParameters();
            parametros.Add("_usuario", usuario, DbType.String, ParameterDirection.Input);

            return await conexion.QueryFirstOrDefaultAsync<Usuario>(
                "sp_usuario_obtener_por_usuario",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return null;
        }
    }

    public async Task<IEnumerable<Rol>> ObtenerRolesAsync(int idUsuario)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            await AsegurarEsquemaYCatalogosOperadoresAsync(conexion);

            var parametros = new DynamicParameters();
            parametros.Add("_id_usuario", idUsuario, DbType.Int32, ParameterDirection.Input);

            return await conexion.QueryAsync<Rol>(
                "sp_usuario_obtener_roles",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<Rol>();
        }
    }

    public async Task<Usuario?> ObtenerPorIdAsync(int id)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            await AsegurarEsquemaYCatalogosOperadoresAsync(conexion);

            var parametros = new DynamicParameters();
            parametros.Add("_id", id, DbType.Int32, ParameterDirection.Input);

            var user = await conexion.QueryFirstOrDefaultAsync<Usuario>(
                "sp_usuario_obtener_por_id",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            if (user != null)
            {
                var extra = await ObtenerOperadorDetalladoPorIdAsync(id);
                if (extra != null)
                {
                    user.Nombre = extra.Nombre;
                    user.Apellido = extra.Apellido;
                    user.EtiquetaArea = extra.EtiquetaArea;
                    user.IdVeterinario = extra.IdVeterinario;
                    user.IdRol = extra.IdRol;
                    user.RolNombre = extra.RolNombre;
                    user.RolDescripcion = extra.RolDescripcion;
                }
            }

            return user;
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return null;
        }
    }

    public async Task<int> CrearAsync(Usuario usuario)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            await AsegurarEsquemaYCatalogosOperadoresAsync(conexion);

            var parametros = new DynamicParameters();
            parametros.Add("_user", usuario.User, DbType.String, ParameterDirection.Input);
            parametros.Add("_password_hash", usuario.PasswordHash, DbType.String, ParameterDirection.Input);
            parametros.Add("_activo", usuario.Activo, DbType.Boolean, ParameterDirection.Input);
            parametros.Add("_fecha_creacion", usuario.FechaCreacion == default ? DateTime.UtcNow : usuario.FechaCreacion, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_ultimo_acceso", usuario.UltimoAcceso, DbType.DateTime, ParameterDirection.Input);

            var nuevoId = await conexion.ExecuteScalarAsync<int>(
                "sp_usuario_crear",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            if (nuevoId > 0)
            {
                await conexion.ExecuteAsync(@"
                    UPDATE `Usuarios`
                    SET `nombre` = @Nombre,
                        `apellido` = @Apellido,
                        `etiqueta_area` = @EtiquetaArea,
                        `id_veterinario` = @IdVeterinario
                    WHERE `IdUsuario` = @IdUsuario;
                ", new
                {
                    IdUsuario = nuevoId,
                    usuario.Nombre,
                    usuario.Apellido,
                    usuario.EtiquetaArea,
                    usuario.IdVeterinario
                });
            }

            return nuevoId;
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return 0;
        }
    }

    public async Task<bool> ActualizarAsync(Usuario usuario)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            await AsegurarEsquemaYCatalogosOperadoresAsync(conexion);

            var parametros = new DynamicParameters();
            parametros.Add("_id", usuario.IdUsuario, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_user", usuario.User, DbType.String, ParameterDirection.Input);
            parametros.Add("_password_hash", usuario.PasswordHash, DbType.String, ParameterDirection.Input);
            parametros.Add("_activo", usuario.Activo, DbType.Boolean, ParameterDirection.Input);
            parametros.Add("_ultimo_acceso", usuario.UltimoAcceso, DbType.DateTime, ParameterDirection.Input);

            var filas = await conexion.ExecuteScalarAsync<int>(
                "sp_usuario_actualizar",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            await conexion.ExecuteAsync(@"
                UPDATE `Usuarios`
                SET `nombre` = COALESCE(@Nombre, `nombre`),
                    `apellido` = COALESCE(@Apellido, `apellido`),
                    `etiqueta_area` = COALESCE(@EtiquetaArea, `etiqueta_area`),
                    `id_veterinario` = @IdVeterinario
                WHERE `IdUsuario` = @IdUsuario;
            ", new
            {
                usuario.IdUsuario,
                usuario.Nombre,
                usuario.Apellido,
                usuario.EtiquetaArea,
                usuario.IdVeterinario
            });

            return filas >= 0;
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return false;
        }
    }

    public async Task<bool> AsignarRolUnicoAsync(int idUsuario, int idRol)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            await AsegurarEsquemaYCatalogosOperadoresAsync(conexion);

            await conexion.ExecuteAsync(
                "DELETE FROM `Usuario_Rol` WHERE `IdUsuario` = @idUsuario;",
                new { idUsuario }
            );

            var parametros = new DynamicParameters();
            parametros.Add("_id_usuario", idUsuario, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_rol", idRol, DbType.Int32, ParameterDirection.Input);

            var filas = await conexion.ExecuteScalarAsync<int>(
                "sp_usuario_rol_crear",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            return filas > 0;
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return false;
        }
    }

    public async Task<bool> ActualizarUltimoAccesoAsync(int idUsuario, DateTime fechaAcceso)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var filas = await conexion.ExecuteAsync(
                "UPDATE `Usuarios` SET `ultimo_acceso` = @fechaAcceso WHERE `IdUsuario` = @idUsuario;",
                new { idUsuario, fechaAcceso }
            );
            return filas >= 0;
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return false;
        }
    }

    public async Task<bool> EliminarAsync(int id)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            await AsegurarEsquemaYCatalogosOperadoresAsync(conexion);

            // Eliminar primero las asignaciones en Usuario_Rol para respetar la FK usuario_rol_ibfk_2
            await conexion.ExecuteAsync(
                "DELETE FROM `Usuario_Rol` WHERE `IdUsuario` = @id;",
                new { id }
            );

            var parametros = new DynamicParameters();
            parametros.Add("_id", id, DbType.Int32, ParameterDirection.Input);

            var filas = await conexion.ExecuteScalarAsync<int>(
                "sp_usuario_eliminar",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            return filas > 0;
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return false;
        }
    }
}
