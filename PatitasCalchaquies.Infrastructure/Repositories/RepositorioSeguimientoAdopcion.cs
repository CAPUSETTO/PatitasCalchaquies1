using System.Data;
using Dapper;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Context;


namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioSeguimientoAdopcion : IRepositorioSeguimientoAdopcion
{
    private readonly IMyConexion _conn;

    public RepositorioSeguimientoAdopcion(IMyConexion conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<SeguimientoAdopcion>> ObtenerTodosAsync()
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            return await conexion.QueryAsync<SeguimientoAdopcion>(
                "sp_seguimiento_adopcion_obtener_todos",
                commandType: CommandType.StoredProcedure
            );
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<SeguimientoAdopcion>();
        }
    }

    public async Task<SeguimientoAdopcion?> ObtenerPorIdAsync(int id)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", id, DbType.Int32, ParameterDirection.Input);

            return await conexion.QueryFirstOrDefaultAsync<SeguimientoAdopcion>(
                "sp_seguimiento_adopcion_obtener_por_id",
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

    public async Task<int> CrearAsync(SeguimientoAdopcion seguimiento)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id_adopcion", seguimiento.IdAdopcion, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_fecha", seguimiento.Fecha, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_tipo", seguimiento.Tipo, DbType.String, ParameterDirection.Input);
            parametros.Add("_resultado", seguimiento.Resultado, DbType.String, ParameterDirection.Input);
            parametros.Add("_observacion", seguimiento.Observacion, DbType.String, ParameterDirection.Input);

            return await conexion.ExecuteScalarAsync<int>(
                "sp_seguimiento_adopcion_crear",
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

    public async Task<bool> ActualizarAsync(SeguimientoAdopcion seguimiento)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", seguimiento.IdSeguimientoAdopcion, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_adopcion", seguimiento.IdAdopcion, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_fecha", seguimiento.Fecha, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_tipo", seguimiento.Tipo, DbType.String, ParameterDirection.Input);
            parametros.Add("_resultado", seguimiento.Resultado, DbType.String, ParameterDirection.Input);
            parametros.Add("_observacion", seguimiento.Observacion, DbType.String, ParameterDirection.Input);

            var filas = await conexion.ExecuteScalarAsync<int>(
                "sp_seguimiento_adopcion_actualizar",
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
                "sp_seguimiento_adopcion_eliminar",
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