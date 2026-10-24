using System.Data;
using Dapper;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Context;


namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioEsterilizacion : IRepositorioEsterilizacion
{
    private readonly IMyConexion _conn;

    public RepositorioEsterilizacion(IMyConexion conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<Esterilizacion>> ObtenerTodasAsync()
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            return await conexion.QueryAsync<Esterilizacion>(
                "sp_esterilizacion_obtener_todas",
                commandType: CommandType.StoredProcedure
            );
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<Esterilizacion>();
        }
    }

    public async Task<Esterilizacion?> ObtenerPorIdAsync(int id)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", id, DbType.Int32, ParameterDirection.Input);

            return await conexion.QueryFirstOrDefaultAsync<Esterilizacion>(
                "sp_esterilizacion_obtener_por_id",
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

    public async Task<int> CrearAsync(Esterilizacion esterilizacion)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id_animal", esterilizacion.IdAnimal, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_veterinario", esterilizacion.IdVeterinario, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_fecha", esterilizacion.Fecha, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_tipo", esterilizacion.Tipo, DbType.String, ParameterDirection.Input);
            parametros.Add("_observacion", esterilizacion.Observacion, DbType.String, ParameterDirection.Input);

            var resultado = await conexion.ExecuteScalarAsync<int>(
                "sp_esterilizacion_crear",
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

    public async Task<bool> ActualizarAsync(Esterilizacion esterilizacion)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", esterilizacion.IdEsterilizacion, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_animal", esterilizacion.IdAnimal, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_veterinario", esterilizacion.IdVeterinario, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_fecha", esterilizacion.Fecha, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_tipo", esterilizacion.Tipo, DbType.String, ParameterDirection.Input);
            parametros.Add("_observacion", esterilizacion.Observacion, DbType.String, ParameterDirection.Input);

            var filas = await conexion.ExecuteScalarAsync<int>(
                "sp_esterilizacion_actualizar",
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
                "sp_esterilizacion_eliminar",
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