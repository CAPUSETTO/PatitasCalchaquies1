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
}