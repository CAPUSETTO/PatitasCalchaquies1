using System.Data;
using Dapper;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Context;


namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioAnimal : IRepositoryAnimal
{
    private readonly IMyConexion _conn;
    public RepositorioAnimal(IMyConexion conn)
    {
        _conn = conn;
    }

    private static async Task AsegurarCatalogosBaseAsync(IDbConnection conexion)
    {
        await conexion.ExecuteAsync(@"
            INSERT IGNORE INTO `Especies` (`IdEspecie`, `nombre`) VALUES (1, 'Canino'), (2, 'Felino');
            INSERT IGNORE INTO `Sexo` (`IdSexo`, `nombre`) VALUES (1, 'Macho'), (2, 'Hembra');
            INSERT IGNORE INTO `Estado_Animal` (`IdEstadoAnimal`, `nombre`) VALUES (1, 'Disponible'), (2, 'En Tratamiento'), (3, 'Adoptado'), (4, 'En Tránsito');
        ");
    }

    public async Task<IEnumerable<Animal>> ObtenerTodosAsync()
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();
            return await _conexion.QueryAsync<Animal>("sp_animal_obtener_todos",
                commandType: CommandType.StoredProcedure);

        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<Animal>();
        } 
    }

    public async Task<Animal> ObtenerPorIdAsync(int id)
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();
            var parametros = new DynamicParameters();
            parametros.Add("_id", id, DbType.Int32, ParameterDirection.Input);

            return await _conexion.QueryFirstOrDefaultAsync<Animal>("sp_animal_obtener_por_id", parametros,
                commandType: CommandType.StoredProcedure);

        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return null;
        }
    }
    
    public async Task<int> CrearAsync(Animal animal)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            await AsegurarCatalogosBaseAsync(conexion);

            int? idRazaValida = animal.IdRaza.HasValue && animal.IdRaza.Value > 0 ? animal.IdRaza.Value : null;
            if (idRazaValida.HasValue)
            {
                var existeRaza = await conexion.ExecuteScalarAsync<int>(
                    "SELECT COUNT(1) FROM `Razas` WHERE `IdRaza` = @idRaza;",
                    new { idRaza = idRazaValida.Value });
                if (existeRaza == 0)
                {
                    idRazaValida = null;
                }
            }

            var parametros = new DynamicParameters();
            parametros.Add("_nombre", animal.Nombre, DbType.String, ParameterDirection.Input);
            parametros.Add("_codigo_identificacion", animal.CodigoIdentificacion, DbType.String, ParameterDirection.Input);
            parametros.Add("_fecha_ingreso", animal.FechaIngreso, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_castrado", animal.Castrado, DbType.Boolean, ParameterDirection.Input);
            parametros.Add("_id_especie", animal.IdEspecie, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_raza", idRazaValida, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_sexo", animal.IdSexo.HasValue && animal.IdSexo.Value > 0 ? animal.IdSexo.Value : 1, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_estado", animal.IdEstado.HasValue && animal.IdEstado.Value > 0 ? animal.IdEstado.Value : 1, DbType.Int32, ParameterDirection.Input);
            
            var resultado = await conexion.ExecuteScalarAsync<int>("sp_animal_crear", parametros, commandType: CommandType.StoredProcedure);
            return resultado;
            
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return 0;
        }
    }

    public async Task<bool> ActualizarAsync(Animal animal)
    {
        try
        {
            using var conexion = await _conn.obtenerConexionAsync();
            await AsegurarCatalogosBaseAsync(conexion);

            int? idRazaValida = animal.IdRaza.HasValue && animal.IdRaza.Value > 0 ? animal.IdRaza.Value : null;
            if (idRazaValida.HasValue)
            {
                var existeRaza = await conexion.ExecuteScalarAsync<int>(
                    "SELECT COUNT(1) FROM `Razas` WHERE `IdRaza` = @idRaza;",
                    new { idRaza = idRazaValida.Value });
                if (existeRaza == 0)
                {
                    idRazaValida = null;
                }
            }

            var parametros = new DynamicParameters();
            parametros.Add("_id", animal.IdAnimal, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_nombre", animal.Nombre, DbType.String, ParameterDirection.Input);
            parametros.Add("_codigo_identificacion", animal.CodigoIdentificacion, DbType.String, ParameterDirection.Input);
            parametros.Add("_fecha_ingreso", animal.FechaIngreso, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("_castrado", animal.Castrado, DbType.Boolean, ParameterDirection.Input);
            parametros.Add("_id_especie", animal.IdEspecie, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_raza", idRazaValida, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_sexo", animal.IdSexo.HasValue && animal.IdSexo.Value > 0 ? animal.IdSexo.Value : 1, DbType.Int32, ParameterDirection.Input);
            parametros.Add("_id_estado", animal.IdEstado.HasValue && animal.IdEstado.Value > 0 ? animal.IdEstado.Value : 1, DbType.Int32, ParameterDirection.Input);
            
            var filas = await conexion.ExecuteScalarAsync<int>("sp_animal_actualizar", parametros, commandType: CommandType.StoredProcedure);
            return filas >= 0;
            
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
            
            var filas = await conexion.ExecuteScalarAsync<int>("sp_animal_eliminar", parametros, commandType: CommandType.StoredProcedure);
            return filas > 0;
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return false;
        }
    }
}
