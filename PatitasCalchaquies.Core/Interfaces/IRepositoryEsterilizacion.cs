using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioEsterilizacion
{
    Task<IEnumerable<Esterilizacion>> ObtenerTodasAsync();
    Task<Esterilizacion?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Esterilizacion esterilizacion);
    Task<bool> ActualizarAsync(Esterilizacion esterilizacion);
    Task<bool> EliminarAsync(int id);
}