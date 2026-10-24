using System.Data;
using Dapper;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Context;


namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioAlimentacion : IRepositorioAlimentacion
{
    private readonly IMyConexion _conn;

    public RepositorioAlimentacion(IMyConexion conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<Alimentacion>> ObtenerTodasAsync()
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            return await conexion.QueryAsync<Alimentacion>(
                "sp_alimentacion_obtener_todas",
                commandType: CommandType.StoredProcedure
            );
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<Alimentacion>();
        }
    }

    public async Task<Alimentacion?> ObtenerPorIdAsync(int id)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", id, DbType.Int32, ParameterDirection.Input);

            return await conexion.QueryFirstOrDefaultAsync<Alimentacion>(
                "sp_alimentacion_obtener_por_id",
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

    public async Task<int> CrearAsync(Alimentacion alimentacion)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id_animal", alimentacion.IdAnimal, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_fecha_hora", alimentacion.FechaHora, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_tipo_alimento", alimentacion.TipoAlimento, DbType.String, ParameterDirection.Input);
            parametros.Add("_cantidad", alimentacion.Cantidad, DbType.Decimal, ParameterDirection.Input);
            parametros.Add("_observacion", alimentacion.Observacion, DbType.String, ParameterDirection.Input);

            var resultado = await conexion.ExecuteScalarAsync<int>(
                "sp_alimentacion_crear",
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

    public async Task<bool> ActualizarAsync(Alimentacion alimentacion)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", alimentacion.IdAlimentacion, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_animal", alimentacion.IdAnimal, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_fecha_hora", alimentacion.FechaHora, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_tipo_alimento", alimentacion.TipoAlimento, DbType.String, ParameterDirection.Input);
            parametros.Add("_cantidad", alimentacion.Cantidad, DbType.Decimal, ParameterDirection.Input);
            parametros.Add("_observacion", alimentacion.Observacion, DbType.String, ParameterDirection.Input);

            var filas = await conexion.ExecuteScalarAsync<int>(
                "sp_alimentacion_actualizar",
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
                "sp_alimentacion_eliminar",
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