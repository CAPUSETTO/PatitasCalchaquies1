using System.Data;
using Dapper;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Context;

namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioDevolucionAdopcion : IRepositorioDevolucionAdopcion
{
    private readonly IMyConexion _conn;

    public RepositorioDevolucionAdopcion(IMyConexion conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<DevolucionAdopcion>> ObtenerTodasAsync()
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            return await conexion.QueryAsync<DevolucionAdopcion>(
                "sp_devolucion_adopcion_obtener_todas",
                commandType: CommandType.StoredProcedure
            );
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<DevolucionAdopcion>();
        }
    }

    public async Task<DevolucionAdopcion?> ObtenerPorIdAsync(int id)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", id, DbType.Int32, ParameterDirection.Input);

            return await conexion.QueryFirstOrDefaultAsync<DevolucionAdopcion>(
                "sp_devolucion_adopcion_obtener_por_id",
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

    public async Task<int> CrearAsync(DevolucionAdopcion devolucion)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id_adopcion", devolucion.IdAdopcion, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_fecha", devolucion.Fecha, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_motivo", devolucion.Motivo, DbType.String, ParameterDirection.Input);
            parametros.Add("_observacion", devolucion.Observacion, DbType.String, ParameterDirection.Input);

            var resultado = await conexion.ExecuteScalarAsync<int>(
                "sp_devolucion_adopcion_crear",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            return resultado;
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return 0;
        }
    }

    public async Task<bool> ActualizarAsync(DevolucionAdopcion devolucion)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", devolucion.IdDevolucionAdopcion, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_adopcion", devolucion.IdAdopcion, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_fecha", devolucion.Fecha, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_motivo", devolucion.Motivo, DbType.String, ParameterDirection.Input);
            parametros.Add("_observacion", devolucion.Observacion, DbType.String, ParameterDirection.Input);

            var filas = await conexion.ExecuteScalarAsync<int>(
                "sp_devolucion_adopcion_actualizar",
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
                "sp_devolucion_adopcion_eliminar",
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