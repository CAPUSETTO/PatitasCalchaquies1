using System.Data;
using Dapper;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Context;


namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioTratamiento : IRepositorioTratamiento
{
    private readonly IMyConexion _conn;

    public RepositorioTratamiento(IMyConexion conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<Tratamiento>> ObtenerTodosAsync()
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            return await conexion.QueryAsync<Tratamiento>(
                "sp_tratamiento_obtener_todos",
                commandType: CommandType.StoredProcedure
            );
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<Tratamiento>();
        }
    }

    public async Task<Tratamiento?> ObtenerPorIdAsync(int id)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", id, DbType.Int32, ParameterDirection.Input);

            return await conexion.QueryFirstOrDefaultAsync<Tratamiento>(
                "sp_tratamiento_obtener_por_id",
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

    public async Task<int> CrearAsync(Tratamiento tratamiento)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id_atencion_veterinaria", tratamiento.IdAtencionVeterinaria, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_descripcion", tratamiento.Descripcion, DbType.String, ParameterDirection.Input);
            parametros.Add("_fecha_inicio", tratamiento.FechaInicio, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_fecha_fin", tratamiento.FechaFin, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_estado", tratamiento.Estado, DbType.String, ParameterDirection.Input);
            parametros.Add("_observacion", tratamiento.Observacion, DbType.String, ParameterDirection.Input);

            return await conexion.ExecuteScalarAsync<int>(
                "sp_tratamiento_crear",
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

    public async Task<bool> ActualizarAsync(Tratamiento tratamiento)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", tratamiento.IdTratamiento, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_atencion_veterinaria", tratamiento.IdAtencionVeterinaria, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_descripcion", tratamiento.Descripcion, DbType.String, ParameterDirection.Input);
            parametros.Add("_fecha_inicio", tratamiento.FechaInicio, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_fecha_fin", tratamiento.FechaFin, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_estado", tratamiento.Estado, DbType.String, ParameterDirection.Input);
            parametros.Add("_observacion", tratamiento.Observacion, DbType.String, ParameterDirection.Input);

            var filas = await conexion.ExecuteScalarAsync<int>(
                "sp_tratamiento_actualizar",
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
                "sp_tratamiento_eliminar",
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