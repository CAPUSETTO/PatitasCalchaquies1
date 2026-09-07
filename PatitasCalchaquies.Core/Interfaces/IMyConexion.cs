using System.Data;
using System.Threading.Tasks;

namespace PatitasCalchaquies.Core.Interfaces
{
    public interface IMyConexion
    {
        IDbConnection obtenerConexion();
        Task<IDbConnection> obtenerConexionAsync();
    }
}