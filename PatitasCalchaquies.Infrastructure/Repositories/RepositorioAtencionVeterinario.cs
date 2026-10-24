using System.Data;
using Dapper;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Context;


namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioAtencionVeterinaria : IRepositorioAtencionVeterinaria
{
    private readonly IMyConexion _conn;

    public RepositorioAtencionVeterinaria(IMyConexion conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<AtencionVeterinaria>> ObtenerTodasAsync()
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            return await conexion.QueryAsync<AtencionVeterinaria>(
                "sp_atencion_veterinaria_obtener_todas",
                commandType: CommandType.StoredProcedure
            );
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<AtencionVeterinaria>();
        }
    }

    public async Task<AtencionVeterinaria?> ObtenerPorIdAsync(int id)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", id, DbType.Int32, ParameterDirection.Input);

            return await conexion.QueryFirstOrDefaultAsync<AtencionVeterinaria>(
                "sp_atencion_veterinaria_obtener_por_id",
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

    public async Task<int> CrearAsync(AtencionVeterinaria atencion)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id_animal", atencion.IdAnimal, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_veterinario", atencion.IdVeterinario, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_fecha", atencion.Fecha, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_motivo", atencion.Motivo, DbType.String, ParameterDirection.Input);

            return await conexion.ExecuteScalarAsync<int>(
                "sp_atencion_veterinaria_crear",
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

    public async Task<bool> ActualizarAsync(AtencionVeterinaria atencion)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", atencion.IdAtencionVeterinaria, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_animal", atencion.IdAnimal, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_veterinario", atencion.IdVeterinario, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_fecha", atencion.Fecha, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_motivo", atencion.Motivo, DbType.String, ParameterDirection.Input);

            var filas = await conexion.ExecuteScalarAsync<int>(
                "sp_atencion_veterinaria_actualizar",
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
                "sp_atencion_veterinaria_eliminar",
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