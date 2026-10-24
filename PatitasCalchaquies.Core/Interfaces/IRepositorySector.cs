using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioSector
{
    Task<IEnumerable<Sector>> ObtenerTodosAsync();
    Task<Sector?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Sector sector);
    Task<bool> ActualizarAsync(Sector sector);
    Task<bool> EliminarAsync(int id);
}