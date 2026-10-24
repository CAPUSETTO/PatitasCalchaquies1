using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioPredio
{
    Task<IEnumerable<Predio>> ObtenerTodosAsync();
    Task<Predio?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Predio predio);
    Task<bool> ActualizarAsync(Predio predio);
    Task<bool> EliminarAsync(int id);
}