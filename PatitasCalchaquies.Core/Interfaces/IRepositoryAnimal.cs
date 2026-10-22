using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositoryAnimal
{
    Task<IEnumerable<Animal>> ObtenerTodosAsync();
    Task<Animal> ObtenerPorIdAsync(int id);
    Task<int>CrearAsync(Animal animal);
    Task<bool> ActualizarAsync(Animal animal);
    Task<bool> EliminarAsync(int id);
}