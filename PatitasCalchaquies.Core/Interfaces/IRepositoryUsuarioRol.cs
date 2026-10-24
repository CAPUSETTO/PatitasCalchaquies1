using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioUsuarioRol
{
    Task<IEnumerable<UsuarioRol>> ObtenerTodosAsync();
    Task<UsuarioRol?> ObtenerPorClaveAsync(int idUsuario, int idRol);
    Task<IEnumerable<UsuarioRol>> ObtenerPorUsuarioAsync(int idUsuario);
    Task<bool> CrearAsync(UsuarioRol entidad);
    Task<bool> EliminarAsync(int idUsuario, int idRol);
}