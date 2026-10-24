using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioIngresoAnimal
{
    Task<IEnumerable<IngresoAnimal>> ObtenerTodosAsync();
    Task<IngresoAnimal?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(IngresoAnimal ingreso);
    Task<bool> ActualizarAsync(IngresoAnimal ingreso);
    Task<bool> EliminarAsync(int id);
}