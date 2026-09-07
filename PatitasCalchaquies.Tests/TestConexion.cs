using PatitasCalchaquies.Infrastructure.Context;
using PatitasCalchaquies.Infrastructure.Contexto;
using System.Data;
using Xunit;

namespace PatitasCalchaquies.Tests
{
    public class TestConexion
    {
        [Fact]
        public async Task ProbarConexionAMySql()
        {
            // Ajusta la contraseña según tu servidor local de MySQL
            string connectionString = "Server=127.0.0.1;Database=PatitasCalchaquies;User Id=root;Password=Fenixss1!;Port=3306;";

            var conexion = new MyConexion(connectionString);
            using var conn = await conexion.obtenerConexionAsync();

            // Verificaciones automáticas
            Assert.NotNull(conn);
            Assert.Equal(ConnectionState.Open, conn.State);
        }
    }
}
