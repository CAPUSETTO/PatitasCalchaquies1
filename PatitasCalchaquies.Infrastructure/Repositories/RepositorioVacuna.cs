using System.ComponentModel;
using System.Data;
using MySqlConnector;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Context;

namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioVacuna : IRepositoryVacuna
{
    private readonly IMyConexion _conn;

    public RepositorioVacuna(IMyConexion conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<Vacuna>> ObtenerTodosAsync()
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_vacunas_obtener_todos";
            
            using IDataReader reader = _comando.ExecuteReader();
            List<Vacuna> listaVacunas = new List<Vacuna>();

            while (reader.Read())
            {
                Vacuna vacuna = new Vacuna ()
                {
                    IdVacuna = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Descripcion = reader.GetString(2)
                };
                listaVacunas.Add(vacuna);
            }
            
            return listaVacunas;
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return new List<Vacuna>();
        }
    }

    public async Task<Vacuna?> ObtenerPorIdAsync(int id)
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_vacunas_obtener_por_id";
            
            IDbDataParameter parametro = _comando.CreateParameter();
            parametro.ParameterName = "@_id";
            parametro.Value = id;
            _comando.Parameters.Add(parametro);
            
            using IDataReader reader = _comando.ExecuteReader();

            if (reader.Read())
            {
                return new Vacuna
                {
                    IdVacuna = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Descripcion = reader.GetString(2)
                };
            }
            return null;
        }
        catch (MySqlException error)
        {
            ArchivoLog.RegistrarErrores(error);
            return null;
        }
    }

    public async Task<IEnumerable<Vacuna>> BuscarAsync(string texto)
    {
        try
        {
            using IDbConnection conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_vacuna_buscar";

            IDbDataParameter parametro = _comando.CreateParameter();
            parametro.ParameterName = "_buscar";
            parametro.Value = texto;
            _comando.Parameters.Add(parametro);

            using IDataReader reader = _comando.ExecuteReader();
            List<Vacuna> lista = new List<Vacuna>();

            while (reader.Read())
            {
                lista.Add(new Vacuna
                {
                    IdVacuna = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Descripcion = reader.GetString(2)
                });
            }

            return lista;
        }
        catch (MySqlException error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<Vacuna>();
        }
    }

    public async Task<int> CrearAsync(Vacuna vacuna)
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_vacuna_crear";

            Action<string, object> AddParam = (name, Value) =>
            {
                var p = _comando.CreateParameter();
                p.ParameterName = name;
                p.Value = Value ?? DBNull.Value;
                _comando.Parameters.Add(p);
            };
            AddParam("@_nombre", vacuna.Nombre);
            AddParam("@_descripcion", vacuna.Descripcion);
            
            object? result = _comando.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : 0;
        }
        catch (MySqlException error)
        {
            ArchivoLog.RegistrarErrores(error);
            return 0;
        }
    }
    
    public async Task<bool> ActualizarAsync(Vacuna vacuna)
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_vacuna_actualizar";

            Action<string, object?> AddParam = (name, value) =>
            {
                var p = _comando.CreateParameter();
                p.ParameterName = name;
                p.Value = value ?? DBNull.Value;
                _comando.Parameters.Add(p);
            };

            AddParam("@_id", vacuna.IdVacuna);
            AddParam("@_nombre", vacuna.Nombre);
            AddParam("@_descripcion", vacuna.Descripcion);

            int filasAfectadas = Convert.ToInt32(_comando.ExecuteScalar());
            return filasAfectadas > 0;
        }
        catch (MySqlException error)
        {
            ArchivoLog.RegistrarErrores(error);
            return false;
        }
    }

    public async Task<bool> EliminarAsync(int id)
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_vacuna_eliminar";

            IDbDataParameter parametro = _comando.CreateParameter();
            parametro.ParameterName = "_id";
            parametro.Value = id;
            _comando.Parameters.Add(parametro);

            int filasAfectadas = Convert.ToInt32(_comando.ExecuteScalar());
            return filasAfectadas > 0;
        }
        catch (MySqlException error)
        {
            ArchivoLog.RegistrarErrores(error);
            return false;
        }
    }
}