using System.Data;
using Dapper;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Context;


namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioUsuarioRol : IRepositorioUsuarioRol
{
    private readonly IMyConexion _conn;

    public RepositorioUsuarioRol(IMyConexion conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<UsuarioRol>> ObtenerTodosAsync()
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            return await conexion.QueryAsync<UsuarioRol>(
                "sp_usuario_rol_obtener_todos",
                commandType: CommandType.StoredProcedure
            );
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<UsuarioRol>();
        }
    }

    public async Task<UsuarioRol?> ObtenerPorClaveAsync(int idUsuario, int idRol)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id_usuario", idUsuario, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_rol", idRol, DbType.Int32, ParameterDirection.Input);

            return await conexion.QueryFirstOrDefaultAsync<UsuarioRol>(
                "sp_usuario_rol_obtener_por_clave",
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

    public async Task<IEnumerable<UsuarioRol>> ObtenerPorUsuarioAsync(int idUsuario)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id_usuario", idUsuario, DbType.Int32, ParameterDirection.Input);

            return await conexion.QueryAsync<UsuarioRol>(
                "sp_usuario_rol_obtener_por_usuario",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<UsuarioRol>();
        }
    }

    public async Task<bool> CrearAsync(UsuarioRol entidad)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id_usuario", entidad.IdUsuario, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_rol", entidad.IdRol, DbType.Int32, ParameterDirection.Input);

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

    public async Task<bool> EliminarAsync(int idUsuario, int idRol)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id_usuario", idUsuario, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_rol", idRol, DbType.Int32, ParameterDirection.Input);

            var filas = await conexion.ExecuteScalarAsync<int>(
                "sp_usuario_rol_eliminar",
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