using System.Data;
using Dapper;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Context;


namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioSector : IRepositorioSector
{
    private readonly IMyConexion _conn;

    public RepositorioSector(IMyConexion conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<Sector>> ObtenerTodosAsync()
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            return await conexion.QueryAsync<Sector>(
                "sp_sector_obtener_todos",
                commandType: CommandType.StoredProcedure
            );
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<Sector>();
        }
    }

    public async Task<Sector?> ObtenerPorIdAsync(int id)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", id, DbType.Int32, ParameterDirection.Input);

            return await conexion.QueryFirstOrDefaultAsync<Sector>(
                "sp_sector_obtener_por_id",
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

    public async Task<int> CrearAsync(Sector sector)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_nombre", sector.Nombre, DbType.String, ParameterDirection.Input);
            parametros.Add("_capacidad", sector.Capacidad, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_descripcion", sector.Descripcion, DbType.String, ParameterDirection.Input);
            parametros.Add("_id_predio", sector.IdPredio, DbType.Int32, ParameterDirection.Input);

            return await conexion.ExecuteScalarAsync<int>(
                "sp_sector_crear",
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

    public async Task<bool> ActualizarAsync(Sector sector)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", sector.IdSector, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_nombre", sector.Nombre, DbType.String, ParameterDirection.Input);
            parametros.Add("_capacidad", sector.Capacidad, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_descripcion", sector.Descripcion, DbType.String, ParameterDirection.Input);
            parametros.Add("_id_predio", sector.IdPredio, DbType.Int32, ParameterDirection.Input);

            var filas = await conexion.ExecuteScalarAsync<int>(
                "sp_sector_actualizar",
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
                "sp_sector_eliminar",
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