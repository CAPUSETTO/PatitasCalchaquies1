using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioAlimentacion
{
    Task<IEnumerable<Alimentacion>> ObtenerTodasAsync();
    Task<Alimentacion?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Alimentacion alimentacion);
    Task<bool> ActualizarAsync(Alimentacion alimentacion);
    Task<bool> EliminarAsync(int id);
}