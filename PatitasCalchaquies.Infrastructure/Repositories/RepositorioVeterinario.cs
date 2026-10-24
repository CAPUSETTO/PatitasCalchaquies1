using System.Data;
using Dapper;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Context;


namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioVeterinario : IRepositorioVeterinario
{
    private readonly IMyConexion _conn;

    public RepositorioVeterinario(IMyConexion conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<Veterinario>> ObtenerTodosAsync()
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            return await conexion.QueryAsync<Veterinario>(
                "sp_veterinario_obtener_todos",
                commandType: CommandType.StoredProcedure
            );
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<Veterinario>();
        }
    }

    public async Task<Veterinario?> ObtenerPorIdAsync(int id)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", id, DbType.Int32, ParameterDirection.Input);

            return await conexion.QueryFirstOrDefaultAsync<Veterinario>(
                "sp_veterinario_obtener_por_id",
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

    public async Task<int> CrearAsync(Veterinario veterinario)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_matricula", veterinario.Matricula, DbType.String, ParameterDirection.Input);
            parametros.Add("_nombre", veterinario.Nombre, DbType.String, ParameterDirection.Input);
            parametros.Add("_apellido", veterinario.Apellido, DbType.String, ParameterDirection.Input);
            parametros.Add("_telefono", veterinario.Telefono, DbType.String, ParameterDirection.Input);

            return await conexion.ExecuteScalarAsync<int>(
                "sp_veterinario_crear",
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

    public async Task<bool> ActualizarAsync(Veterinario veterinario)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", veterinario.IdVeterinario, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_matricula", veterinario.Matricula, DbType.String, ParameterDirection.Input);
            parametros.Add("_nombre", veterinario.Nombre, DbType.String, ParameterDirection.Input);
            parametros.Add("_apellido", veterinario.Apellido, DbType.String, ParameterDirection.Input);
            parametros.Add("_telefono", veterinario.Telefono, DbType.String, ParameterDirection.Input);

            var filas = await conexion.ExecuteScalarAsync<int>(
                "sp_veterinario_actualizar",
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
                "sp_veterinario_eliminar",
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