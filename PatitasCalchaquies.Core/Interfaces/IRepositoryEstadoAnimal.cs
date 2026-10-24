using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioEstadoAnimal
{
    Task<IEnumerable<EstadoAnimal>> ObtenerTodosAsync();
    Task<EstadoAnimal?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(EstadoAnimal estado);
    Task<bool> ActualizarAsync(EstadoAnimal estado);
    Task<bool> EliminarAsync(int id);
}