using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioVacunacion
{
    Task<IEnumerable<Vacunacion>> ObtenerTodasAsync();
    Task<Vacunacion?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Vacunacion vacunacion);
    Task<bool> ActualizarAsync(Vacunacion vacunacion);
    Task<bool> EliminarAsync(int id);
}