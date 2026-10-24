using System.Data;
using Dapper;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Context;


namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioSolicitudAdopcion : IRepositorioSolicitudAdopcion
{
    private readonly IMyConexion _conn;

    public RepositorioSolicitudAdopcion(IMyConexion conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<SolicitudAdopcion>> ObtenerTodasAsync()
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            return await conexion.QueryAsync<SolicitudAdopcion>(
                "sp_solicitud_adopcion_obtener_todas",
                commandType: CommandType.StoredProcedure
            );
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<SolicitudAdopcion>();
        }
    }

    public async Task<SolicitudAdopcion?> ObtenerPorIdAsync(int id)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", id, DbType.Int32, ParameterDirection.Input);

            return await conexion.QueryFirstOrDefaultAsync<SolicitudAdopcion>(
                "sp_solicitud_adopcion_obtener_por_id",
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

    public async Task<int> CrearAsync(SolicitudAdopcion solicitud)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id_persona", solicitud.IdPersona, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_animal", solicitud.IdAnimal, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_fecha", solicitud.Fecha, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_estado", solicitud.Estado, DbType.String, ParameterDirection.Input);
            parametros.Add("_observacion", solicitud.Observacion, DbType.String, ParameterDirection.Input);

            var resultado = await conexion.ExecuteScalarAsync<int>(
                "sp_solicitud_adopcion_crear",
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

    public async Task<bool> ActualizarAsync(SolicitudAdopcion solicitud)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", solicitud.IdSolicitudAdopcion, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_persona", solicitud.IdPersona, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_animal", solicitud.IdAnimal, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_fecha", solicitud.Fecha, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_estado", solicitud.Estado, DbType.String, ParameterDirection.Input);
            parametros.Add("_observacion", solicitud.Observacion, DbType.String, ParameterDirection.Input);

            var filas = await conexion.ExecuteScalarAsync<int>(
                "sp_solicitud_adopcion_actualizar",
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
                "sp_solicitud_adopcion_eliminar",
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