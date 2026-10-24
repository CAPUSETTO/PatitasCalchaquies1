using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioRol
{
    Task<IEnumerable<Rol>> ObtenerTodosAsync();
    Task<Rol?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Rol rol);
    Task<bool> ActualizarAsync(Rol rol);
    Task<bool> EliminarAsync(int id);
}