using MySqlConnector;
using PatitasCalchaquies.Core.Interfaces;
using System.Threading.Tasks;

namespace PatitasCalchaquies.Infrastructure.Context
{
    public class MyConexion : IMyConexion
    {
        private readonly string _cadenaDeConexion;

        public MyConexion(string cadena)
        {
            _cadenaDeConexion = cadena;
        }

        public System.Data.IDbConnection obtenerConexion()
        {
            MySqlConnection conn = new MySqlConnection(_cadenaDeConexion);
            conn.Open();
            return conn;
        }

        public async Task<System.Data.IDbConnection> obtenerConexionAsync()
        {
            MySqlConnection conn = new MySqlConnection(_cadenaDeConexion);
            try
            {
                await conn.OpenAsync();
                return conn;
            }
            catch (Exception error)
            {
                ArchivoLog.RegistrarErrores(error);
                conn.Dispose();
                throw;
            }
        }
    }
}