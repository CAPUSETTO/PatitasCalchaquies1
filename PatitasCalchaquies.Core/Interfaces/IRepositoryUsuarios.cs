using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioUsuario
{
    Task<IEnumerable<Usuario>> ObtenerTodosAsync();
    Task<Usuario?> ObtenerPorUsuarioAsync(string usuario);
    Task<Usuario?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Usuario usuario);
    Task<bool> ActualizarAsync(Usuario usuario);
    Task<bool> EliminarAsync(int id);
    Task<IEnumerable<Rol>> ObtenerRolesAsync(int idUsuario);
    Task<IEnumerable<Usuario>> ObtenerOperadoresDetalladosAsync();
    Task<Usuario?> ObtenerOperadorDetalladoPorIdAsync(int idUsuario);
    Task<bool> AsignarRolUnicoAsync(int idUsuario, int idRol);
    Task<bool> ActualizarUltimoAccesoAsync(int idUsuario, DateTime fechaAcceso);
}
