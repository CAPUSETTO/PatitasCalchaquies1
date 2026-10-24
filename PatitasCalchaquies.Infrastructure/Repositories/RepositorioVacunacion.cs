using System.Data;
using Dapper;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Context;


namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioVacunacion : IRepositorioVacunacion
{
    private readonly IMyConexion _conn;

    public RepositorioVacunacion(IMyConexion conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<Vacunacion>> ObtenerTodasAsync()
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            return await conexion.QueryAsync<Vacunacion>(
                "sp_vacunacion_obtener_todas",
                commandType: CommandType.StoredProcedure
            );
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<Vacunacion>();
        }
    }

    public async Task<Vacunacion?> ObtenerPorIdAsync(int id)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", id, DbType.Int32, ParameterDirection.Input);

            return await conexion.QueryFirstOrDefaultAsync<Vacunacion>(
                "sp_vacunacion_obtener_por_id",
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

    public async Task<int> CrearAsync(Vacunacion vacunacion)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id_animal", vacunacion.IdAnimal, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_vacuna", vacunacion.IdVacuna, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_fecha", vacunacion.Fecha, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_proxima_fecha", vacunacion.ProximaFecha, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_observacion", vacunacion.Observacion, DbType.String, ParameterDirection.Input);

            return await conexion.ExecuteScalarAsync<int>(
                "sp_vacunacion_crear",
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

    public async Task<bool> ActualizarAsync(Vacunacion vacunacion)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", vacunacion.IdVacunacion, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_animal", vacunacion.IdAnimal, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_vacuna", vacunacion.IdVacuna, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_fecha", vacunacion.Fecha, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_proxima_fecha", vacunacion.ProximaFecha, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_observacion", vacunacion.Observacion, DbType.String, ParameterDirection.Input);

            var filas = await conexion.ExecuteScalarAsync<int>(
                "sp_vacunacion_actualizar",
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
                "sp_vacunacion_eliminar",
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