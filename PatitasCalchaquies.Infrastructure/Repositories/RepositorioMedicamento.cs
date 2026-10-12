using System.ComponentModel;
using System.Data;
using MySqlConnector;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Context;

namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioMedicamento : IRepositoryMedicamento
{
    private readonly IMyConexion _conn;

    public RepositorioMedicamento(IMyConexion conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<Medicamento>> ObtenerTodosAsync()
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_medicamentos_obtener_todos";
            
            using IDataReader reader = _comando.ExecuteReader();
            List<Medicamento> listaMedicamentos = new List<Medicamento>();

            while (reader.Read())
            {
                Medicamento medicamento = new Medicamento()
                {
                    IdMedicamento = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Descripcion = reader.GetString(2)
                };
                listaMedicamentos.Add(medicamento);
            }
            
            return listaMedicamentos;
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return new List<Medicamento>();
        }
    }

    public async Task<Medicamento?> ObtenerPorIdAsync(int id)
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_medicamentos_obtener_por_id";
            
            IDbDataParameter parametro = _comando.CreateParameter();
            parametro.ParameterName = "@_id";
            parametro.Value = id;
            _comando.Parameters.Add(parametro);
            
            using IDataReader reader = _comando.ExecuteReader();

            if (reader.Read())
            {
                return new Medicamento
                {
                    IdMedicamento = reader.GetInt32(0),
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

    public async Task<IEnumerable<Medicamento>> BuscarAsync(string texto)
    {
        try
        {
            using IDbConnection conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_medicamento_buscar";

            IDbDataParameter parametro = _comando.CreateParameter();
            parametro.ParameterName = "_buscar";
            parametro.Value = texto;
            _comando.Parameters.Add(parametro);

            using IDataReader reader = _comando.ExecuteReader();
            List<Medicamento> lista = new List<Medicamento>();

            while (reader.Read())
            {
                lista.Add(new Medicamento
                {
                    IdMedicamento = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Descripcion = reader.GetString(2)
                });
            }

            return lista;
        }
        catch (MySqlException error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<Medicamento>();
        }
    }

    public async Task<int> CrearAsync(Medicamento medicamento)
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_medicamento_crear";

            Action<string, object> AddParam = (name, Value) =>
            {
                var p = _comando.CreateParameter();
                p.ParameterName = name;
                p.Value = Value ?? DBNull.Value;
                _comando.Parameters.Add(p);
            };
            AddParam("@_nombre", medicamento.Nombre);
            AddParam("@_descripcion", medicamento.Descripcion);
            
            object? result = _comando.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : 0;
        }
        catch (MySqlException error)
        {
            ArchivoLog.RegistrarErrores(error);
            return 0;
        }
    }
    
    public async Task<bool> ActualizarAsync(Medicamento medicamento)
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_medicamento_actualizar";

            Action<string, object?> AddParam = (name, value) =>
            {
                var p = _comando.CreateParameter();
                p.ParameterName = name;
                p.Value = value ?? DBNull.Value;
                _comando.Parameters.Add(p);
            };

            AddParam("@_id", medicamento.IdMedicamento);
            AddParam("@_nombre", medicamento.Nombre);
            AddParam("@_descripcion", medicamento.Descripcion);

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
            _comando.CommandText = "sp_medicamento_eliminar";

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