using System.Data;
using PatitasCalchaquies.Infrastructure.Context;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioPersona : IRepositoryPersona
{
    private readonly IMyConexion _conn;

    public RepositorioPersona(IMyConexion conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<Persona>> ObtenerTodosAsync()
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_persona_obtener_todos";

            using IDataReader reader = _comando.ExecuteReader();
            List<Persona> listaPersonas = new List<Persona>();

            while (reader.Read())
            {
                Persona persona = new Persona()
                {
                    IdPersona = reader.GetInt32(0),
                    Apellido = reader.GetString(1),
                    Nombre = reader.GetString(2),
                    Dni = reader.GetString(3),
                    Telefono = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Email = reader.IsDBNull(5) ? null : reader.GetString(5),
                    Direccion = reader.IsDBNull(6) ? null : reader.GetString(6),
                    FechaAlta = reader.GetDateTime(7),
                    Activo = reader.GetBoolean(8)
                };
                listaPersonas.Add(persona);
            }

            return listaPersonas;
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return new List<Persona>();
        }
    }

    public async Task<Persona?> ObtenerPorDniAsync(string dni)
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_persona_obtener_por_dni";

            IDbDataParameter parametro = _comando.CreateParameter();
            parametro.ParameterName = "@_dni";
            parametro.Value = dni;
            _comando.Parameters.Add(parametro);
            
            using IDataReader reader = _comando.ExecuteReader();

            if (reader.Read())
            {
                return new Persona()
                {
                    IdPersona = reader.GetInt32(0),
                    Apellido = reader.GetString(1),
                    Nombre = reader.GetString(2),
                    Dni = reader.GetString(3),
                    Telefono = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Email = reader.IsDBNull(5) ? null : reader.GetString(5),
                    Direccion = reader.IsDBNull(6) ? null : reader.GetString(6),
                    FechaAlta = reader.GetDateTime(7),
                    Activo = reader.GetBoolean(8)
                };
            }

            return null;
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return null;
        }
    }

    public async Task<Persona?> ObtenerPorIdAsync(int idPersona)
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_persona_obtener_por_id";

            IDbDataParameter parametro = _comando.CreateParameter();
            parametro.ParameterName = "@_id";
            parametro.Value = idPersona;
            _comando.Parameters.Add(parametro);
            
            using IDataReader reader = _comando.ExecuteReader();

            if (reader.Read())
            {
                return new Persona()
                {
                    IdPersona = reader.GetInt32(0),
                    Apellido = reader.GetString(1),
                    Nombre = reader.GetString(2),
                    Dni = reader.GetString(3),
                    Telefono = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Email = reader.IsDBNull(5) ? null : reader.GetString(5),
                    Direccion = reader.IsDBNull(6) ? null : reader.GetString(6),
                    FechaAlta = reader.GetDateTime(7),
                    Activo = reader.GetBoolean(8)
                };
            }

            return null;
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return null;
        }
    }
    
    public async Task<int> CrearPersonaAsync(Persona persona)
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_persona_crear";

            Action<string, object> AddParam = (name, Value) =>
            {
                var p = _comando.CreateParameter();
                p.ParameterName = name;
                p.Value = Value;
                _comando.Parameters.Add(p);
            };
            
            AddParam("@_apellido", persona.Apellido);
            AddParam("@_nombre", persona.Nombre);
            AddParam("@_dni", persona.Dni);
            AddParam("@_telefono", persona.Telefono);
            AddParam("@_email", persona.Email);
            AddParam("@_direccion", persona.Direccion);
            
            object result = _comando.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : 0;
            
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return 0;
        }
    }

    public async Task<bool> ActualizarPersonaAsync(Persona persona)
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_persona_actualizar";

            Action<string, object> AddParam = (name, Value) =>
            {
                var p = _comando.CreateParameter();
                p.ParameterName = name;
                p.Value = Value;
                _comando.Parameters.Add(p);
            };

            AddParam("@_id", persona.IdPersona);
            AddParam("@_apellido", persona.Apellido);
            AddParam("@_nombre", persona.Nombre);
            AddParam("@_dni", persona.Dni);
            AddParam("@_telefono", persona.Telefono);
            AddParam("@_email", persona.Email);
            AddParam("@_direccion", persona.Direccion);
            AddParam("@_activo", persona.Activo);

            int filasAfectadas = _comando.ExecuteNonQuery();
            return filasAfectadas > 0;
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return false;
        }
    }

    public async Task<bool> EliminarPersona(int id)
    {
        try
        {
            using  IDbConnection _conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_persona_eliminar";
            
            var parametro = _comando.CreateParameter();
            parametro.ParameterName = "@_id";
            parametro.Value = id;
            _comando.Parameters.Add(parametro);
            
            int filasAfectadas = _comando.ExecuteNonQuery();
            return filasAfectadas > 0;
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return false;
        }
    }
    
    public async Task<bool> ExisteDniAsync(string dni)
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_persona_existe_dni";

            var parametro = _comando.CreateParameter();
            parametro.ParameterName = "@_dni";
            parametro.Value = dni ?? (object)DBNull.Value;
            _comando.Parameters.Add(parametro);
            
            object result = _comando.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                return Convert.ToInt32(result) == 1;
            }
            return false;
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return false;
        }
    }

    public async Task<bool> ExisteEmailAsync(string email)
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_persona_existe_email";
            
            var parametro = _comando.CreateParameter();
            parametro.ParameterName = "@_email";
            parametro.Value = email ?? (object)DBNull.Value;
            _comando.Parameters.Add(parametro);
            
            object result = _comando.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                return Convert.ToInt32(result) == 1;
            }
            return false;
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return false;
        }
    }
    
    public async Task<IEnumerable<Persona>> BuscarPersona(string nombre)
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_persona_buscar";

            var parametro = _comando.CreateParameter();
            parametro.ParameterName = "@_buscar";
            parametro.Value = nombre ?? (object)DBNull.Value;
            _comando.Parameters.Add(parametro);

            using IDataReader reader = _comando.ExecuteReader();
            List<Persona> listaPersonas = new List<Persona>();

            while (reader.Read())
            {
                Persona persona = new Persona
                {
                    IdPersona = reader.GetInt32(0),
                    Apellido = reader.GetString(1),
                    Nombre = reader.GetString(2),
                    Dni = reader.GetString(3),
                    Telefono = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Email = reader.IsDBNull(5) ? null : reader.GetString(5),
                    Direccion = reader.IsDBNull(6) ? null : reader.GetString(6),
                    FechaAlta = reader.GetDateTime(7),
                    Activo = reader.GetBoolean(8)
                };
                listaPersonas.Add(persona);
            }
            return listaPersonas;
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return new List<Persona>();
        }
    }
}