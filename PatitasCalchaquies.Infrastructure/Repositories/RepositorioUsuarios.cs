using System.Data;
using Dapper;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Context;


namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioUsuario : IRepositorioUsuario
{
    private readonly IMyConexion _conn;

    public RepositorioUsuario(IMyConexion conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
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
    
    public async Task<Usuario?> ObtenerPorUsuarioAsync(string usuario)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
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
            var parametros = new DynamicParameters();
            parametros.Add("_id", id, DbType.Int32, ParameterDirection.Input);

            return await conexion.QueryFirstOrDefaultAsync<Usuario>(
                "sp_usuario_obtener_por_id",
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

    public async Task<int> CrearAsync(Usuario usuario)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_user", usuario.User, DbType.String, ParameterDirection.Input);
            parametros.Add("_password_hash", usuario.PasswordHash, DbType.String, ParameterDirection.Input);
            parametros.Add("_activo", usuario.Activo, DbType.Boolean, ParameterDirection.Input);
            parametros.Add("_fecha_creacion", usuario.FechaCreacion, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_ultimo_acceso", usuario.UltimoAcceso, DbType.DateTime, ParameterDirection.Input);

            return await conexion.ExecuteScalarAsync<int>(
                "sp_usuario_crear",
                parametros,
                commandType: CommandType.StoredProcedure
            );
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

            return filas > 0;
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