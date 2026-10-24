using System.Data;
using Dapper;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Context;


namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioTratamientoMedicamento : IRepositorioTratamientoMedicamento
{
    private readonly IMyConexion _conn;

    public RepositorioTratamientoMedicamento(IMyConexion conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<TratamientoMedicamento>> ObtenerTodosAsync()
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            return await conexion.QueryAsync<TratamientoMedicamento>(
                "sp_tratamiento_medicamento_obtener_todos",
                commandType: CommandType.StoredProcedure
            );
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<TratamientoMedicamento>();
        }
    }

    public async Task<TratamientoMedicamento?> ObtenerPorIdAsync(int id)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", id, DbType.Int32, ParameterDirection.Input);

            return await conexion.QueryFirstOrDefaultAsync<TratamientoMedicamento>(
                "sp_tratamiento_medicamento_obtener_por_id",
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

    public async Task<int> CrearAsync(TratamientoMedicamento entidad)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id_medicamento", entidad.IdMedicamento, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_tratamiento", entidad.IdTratamiento, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_dosis", entidad.Dosis, DbType.String, ParameterDirection.Input);
            parametros.Add("_frecuencia", entidad.Frecuencia, DbType.String, ParameterDirection.Input);
            parametros.Add("_duracion", entidad.Duracion, DbType.String, ParameterDirection.Input);
            parametros.Add("_observacion", entidad.Observacion, DbType.String, ParameterDirection.Input);

            return await conexion.ExecuteScalarAsync<int>(
                "sp_tratamiento_medicamento_crear",
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

    public async Task<bool> ActualizarAsync(TratamientoMedicamento entidad)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", entidad.IdTratamientoMedico, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_medicamento", entidad.IdMedicamento, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_tratamiento", entidad.IdTratamiento, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_dosis", entidad.Dosis, DbType.String, ParameterDirection.Input);
            parametros.Add("_frecuencia", entidad.Frecuencia, DbType.String, ParameterDirection.Input);
            parametros.Add("_duracion", entidad.Duracion, DbType.String, ParameterDirection.Input);
            parametros.Add("_observacion", entidad.Observacion, DbType.String, ParameterDirection.Input);

            var filas = await conexion.ExecuteScalarAsync<int>(
                "sp_tratamiento_medicamento_actualizar",
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
                "sp_tratamiento_medicamento_eliminar",
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