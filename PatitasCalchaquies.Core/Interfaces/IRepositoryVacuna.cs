using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositoryVacuna
{
    Task<IEnumerable<Vacuna>> ObtenerTodosAsync();
    Task<Vacuna> ObtenerPorIdAsync(int id);
    Task<IEnumerable<Vacuna>> BuscarAsync(string texto);
    Task<int> CrearAsync (Vacuna vacuna);
    Task<bool> ActualizarAsync (Vacuna vacuna);
    Task<bool> EliminarAsync(int id);
}