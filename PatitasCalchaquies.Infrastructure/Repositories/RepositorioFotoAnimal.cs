using System.Data;
using Dapper;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Context;


namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioFotoAnimal : IRepositorioFotoAnimal
{
    private readonly IMyConexion _conn;

    public RepositorioFotoAnimal(IMyConexion conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<FotoAnimal>> ObtenerTodasAsync()
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            return await conexion.QueryAsync<FotoAnimal>(
                "sp_foto_animal_obtener_todas",
                commandType: CommandType.StoredProcedure
            );
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<FotoAnimal>();
        }
    }

    public async Task<FotoAnimal?> ObtenerPorIdAsync(int id)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", id, DbType.Int32, ParameterDirection.Input);

            return await conexion.QueryFirstOrDefaultAsync<FotoAnimal>(
                "sp_foto_animal_obtener_por_id",
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

    public async Task<int> CrearAsync(FotoAnimal foto)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id_animal", foto.IdAnimal, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_ruta", foto.Ruta, DbType.String, ParameterDirection.Input);
            parametros.Add("_es_principal", foto.EsPrincipal, DbType.Boolean, ParameterDirection.Input);
            parametros.Add("_fecha", foto.Fecha, DbType.DateTime, ParameterDirection.Input);

            return await conexion.ExecuteScalarAsync<int>(
                "sp_foto_animal_crear",
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

    public async Task<bool> ActualizarAsync(FotoAnimal foto)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", foto.IdFotoAnimal, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_animal", foto.IdAnimal, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_ruta", foto.Ruta, DbType.String, ParameterDirection.Input);
            parametros.Add("_es_principal", foto.EsPrincipal, DbType.Boolean, ParameterDirection.Input);
            parametros.Add("_fecha", foto.Fecha, DbType.DateTime, ParameterDirection.Input);

            var filas = await conexion.ExecuteScalarAsync<int>(
                "sp_foto_animal_actualizar",
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
                "sp_foto_animal_eliminar",
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