using System.Data;
using Dapper;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Context;


namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioAlojamientoAnimal : IRepositorioAlojamientoAnimal
{
    private readonly IMyConexion _conn;

    public RepositorioAlojamientoAnimal(IMyConexion conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<AlojamientoAnimal>> ObtenerTodosAsync()
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            return await conexion.QueryAsync<AlojamientoAnimal>(
                "sp_alojamiento_animal_obtener_todos",
                commandType: CommandType.StoredProcedure
            );
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<AlojamientoAnimal>();
        }
    }

    public async Task<AlojamientoAnimal?> ObtenerPorIdAsync(int id)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", id, DbType.Int32, ParameterDirection.Input);

            return await conexion.QueryFirstOrDefaultAsync<AlojamientoAnimal>(
                "sp_alojamiento_animal_obtener_por_id",
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

    public async Task<int> CrearAsync(AlojamientoAnimal alojamiento)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id_animal", alojamiento.IdAnimal, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_sector", alojamiento.IdSector, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_fecha_ingreso", alojamiento.FechaIngreso, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_fecha_salida", alojamiento.FechaSalida, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_observacion", alojamiento.Observacion, DbType.String, ParameterDirection.Input);

            return await conexion.ExecuteScalarAsync<int>(
                "sp_alojamiento_animal_crear",
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

    public async Task<bool> ActualizarAsync(AlojamientoAnimal alojamiento)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", alojamiento.IdAlojamiento, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_animal", alojamiento.IdAnimal, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_sector", alojamiento.IdSector, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_fecha_ingreso", alojamiento.FechaIngreso, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_fecha_salida", alojamiento.FechaSalida, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_observacion", alojamiento.Observacion, DbType.String, ParameterDirection.Input);

            var filas = await conexion.ExecuteScalarAsync<int>(
                "sp_alojamiento_animal_actualizar",
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
                "sp_alojamiento_animal_eliminar",
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