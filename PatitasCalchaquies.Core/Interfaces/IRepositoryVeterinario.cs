using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioVeterinario
{
    Task<IEnumerable<Veterinario>> ObtenerTodosAsync();
    Task<Veterinario?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Veterinario veterinario);
    Task<bool> ActualizarAsync(Veterinario veterinario);
    Task<bool> EliminarAsync(int id);
}