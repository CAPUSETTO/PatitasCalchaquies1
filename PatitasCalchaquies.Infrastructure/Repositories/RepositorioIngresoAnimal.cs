using System.Data;
using Dapper;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Context;


namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioIngresoAnimal : IRepositorioIngresoAnimal
{
    private readonly IMyConexion _conn;

    public RepositorioIngresoAnimal(IMyConexion conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<IngresoAnimal>> ObtenerTodosAsync()
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            return await conexion.QueryAsync<IngresoAnimal>(
                "sp_ingreso_animal_obtener_todos",
                commandType: CommandType.StoredProcedure
            );
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<IngresoAnimal>();
        }
    }

    public async Task<IngresoAnimal?> ObtenerPorIdAsync(int id)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", id, DbType.Int32, ParameterDirection.Input);

            return await conexion.QueryFirstOrDefaultAsync<IngresoAnimal>(
                "sp_ingreso_animal_obtener_por_id",
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

    public async Task<int> CrearAsync(IngresoAnimal ingreso)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id_animal", ingreso.IdAnimal, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_fecha", ingreso.Fecha, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_motivo", ingreso.Motivo, DbType.String, ParameterDirection.Input);
            parametros.Add("_tipo_ingreso", ingreso.TipoIngreso, DbType.String, ParameterDirection.Input);
            parametros.Add("_lugar_origen", ingreso.LugarOrigen, DbType.String, ParameterDirection.Input);
            parametros.Add("_observacion", ingreso.Observacion, DbType.String, ParameterDirection.Input);

            var resultado = await conexion.ExecuteScalarAsync<int>(
                "sp_ingreso_animal_crear",
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

    public async Task<bool> ActualizarAsync(IngresoAnimal ingreso)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", ingreso.IdIngresoAnimal, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_animal", ingreso.IdAnimal, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_fecha", ingreso.Fecha, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_motivo", ingreso.Motivo, DbType.String, ParameterDirection.Input);
            parametros.Add("_tipo_ingreso", ingreso.TipoIngreso, DbType.String, ParameterDirection.Input);
            parametros.Add("_lugar_origen", ingreso.LugarOrigen, DbType.String, ParameterDirection.Input);
            parametros.Add("_observacion", ingreso.Observacion, DbType.String, ParameterDirection.Input);

            var filas = await conexion.ExecuteScalarAsync<int>(
                "sp_ingreso_animal_actualizar",
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
                "sp_ingreso_animal_eliminar",
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