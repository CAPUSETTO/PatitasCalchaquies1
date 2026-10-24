using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioAlojamientoAnimal
{
    Task<IEnumerable<AlojamientoAnimal>> ObtenerTodosAsync();
    Task<AlojamientoAnimal?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(AlojamientoAnimal alojamiento);
    Task<bool> ActualizarAsync(AlojamientoAnimal alojamiento);
    Task<bool> EliminarAsync(int id);
}