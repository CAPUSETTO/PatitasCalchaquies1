using System.ComponentModel;
using System.Data;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Context;

namespace PatitasCalchaquies.Infrastructure.Repositories;

public class RepositorioRaza : IRepositoryRaza
{
    private readonly IMyConexion _conn;

    public RepositorioRaza(IMyConexion conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<Raza>> ObtenerTodasAsync()
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_raza_obtener_todas";
            
            using IDataReader reader = _comando.ExecuteReader();
            List<Raza> listaRazas = new List<Raza>();
            while (reader.Read())
            {
                Raza raza = new Raza()
                {
                    IdRaza = reader.GetInt32(0),
                    IdEspecie = reader.GetInt32(1),
                    Nombre = reader.GetString(2)
                };
                listaRazas.Add(raza);
            }
            return listaRazas;
        }
        catch (Exception error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<Raza>();
        }
    }
    
    public async Task<IEnumerable<Raza>> ObtenerPorEspecieAsync(int idEspecie)
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();
            using IDbCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_raza_obtener_por_especie";

            Action<string, object?> AddParam = (name, value) =>
            {
                var p = _comando.CreateParameter();
                p.ParameterName = name;
                p.Value = value ?? DBNull.Value;
                _comando.Parameters.Add(p);
            };

            AddParam("@_id_especie", idEspecie);

            using IDataReader reader = _comando.ExecuteReader();
            var lista = new List<Raza>();

            while (reader.Read())
            {
                lista.Add(new Raza
                {
                    IdRaza = reader.GetInt32(0),
                    IdEspecie = reader.GetInt32(1),
                    Nombre = reader.GetString(2)
                });
            }

            return lista;
        }
        catch (MySqlException error)
        {
            ArchivoLog.RegistrarErrores(error);
            return Enumerable.Empty<Raza>();
        }
    }
    
    public async Task<int> CrearRazaAsync(Raza raza)
    {
        try
        {
            using IDbConnection _conexion = await _conn.obtenerConexionAsync();

            // Garantizar que las especies base existan para no fallar por FK razas_ibfk_1
            using (IDbCommand seedCmd = _conexion.CreateCommand())
            {
                seedCmd.CommandType = CommandType.Text;
                seedCmd.CommandText = "INSERT IGNORE INTO `Especies` (`IdEspecie`, `nombre`) VALUES (1, 'Canino'), (2, 'Felino');";
                seedCmd.ExecuteNonQuery();
            }

            using IDbCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.CommandText = "sp_raza_crear";

            Action<string, object?> AddParam = (name, value) =>
            {
                var p = _comando.CreateParameter();
                p.ParameterName = name;
                p.Value = value ?? DBNull.Value;
                _comando.Parameters.Add(p);
            };

            AddParam("@_id_especie", raza.IdEspecie);
            AddParam("@_nombre", raza.Nombre);

            object? result = _comando.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : 0;
        }
        catch (MySqlException error)
        {
            ArchivoLog.RegistrarErrores(error);
            return 0;
        }
    }
}
