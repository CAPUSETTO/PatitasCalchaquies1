using System.Data;
using Dapper;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Context;


namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioPlanAlimentacion : IRepositorioPlanAlimentacion
{
    private readonly IMyConexion _conn;

    public RepositorioPlanAlimentacion(IMyConexion conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<PlanAlimentacion>> ObtenerTodosAsync()
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            return await conexion.QueryAsync<PlanAlimentacion>(
                "sp_plan_alimentacion_obtener_todos",
                commandType: CommandType.StoredProcedure
            );
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<PlanAlimentacion>();
        }
    }

    public async Task<PlanAlimentacion?> ObtenerPorIdAsync(int id)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", id, DbType.Int32, ParameterDirection.Input);

            return await conexion.QueryFirstOrDefaultAsync<PlanAlimentacion>(
                "sp_plan_alimentacion_obtener_por_id",
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

    public async Task<int> CrearAsync(PlanAlimentacion plan)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id_animal", plan.IdAnimal, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_tipo_alimento", plan.TipoAlimento, DbType.String, ParameterDirection.Input);
            parametros.Add("_cantidad", plan.Cantidad, DbType.Decimal, ParameterDirection.Input);
            parametros.Add("_frecuencia", plan.Frecuencia, DbType.String, ParameterDirection.Input);
            parametros.Add("_observacion", plan.Observacion, DbType.String, ParameterDirection.Input);
            parametros.Add("_fecha_desde", plan.FechaDesde, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_fecha_hasta", plan.FechaHasta, DbType.DateTime, ParameterDirection.Input);

            return await conexion.ExecuteScalarAsync<int>(
                "sp_plan_alimentacion_crear",
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

    public async Task<bool> ActualizarAsync(PlanAlimentacion plan)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", plan.IdPlan, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_animal", plan.IdAnimal, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_tipo_alimento", plan.TipoAlimento, DbType.String, ParameterDirection.Input);
            parametros.Add("_cantidad", plan.Cantidad, DbType.Decimal, ParameterDirection.Input);
            parametros.Add("_frecuencia", plan.Frecuencia, DbType.String, ParameterDirection.Input);
            parametros.Add("_observacion", plan.Observacion, DbType.String, ParameterDirection.Input);
            parametros.Add("_fecha_desde", plan.FechaDesde, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_fecha_hasta", plan.FechaHasta, DbType.DateTime, ParameterDirection.Input);

            var filas = await conexion.ExecuteScalarAsync<int>(
                "sp_plan_alimentacion_actualizar",
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
                "sp_plan_alimentacion_eliminar",
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