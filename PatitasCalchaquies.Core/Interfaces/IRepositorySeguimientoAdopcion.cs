using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioSeguimientoAdopcion
{
    Task<IEnumerable<SeguimientoAdopcion>> ObtenerTodosAsync();
    Task<SeguimientoAdopcion?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(SeguimientoAdopcion seguimiento);
    Task<bool> ActualizarAsync(SeguimientoAdopcion seguimiento);
    Task<bool> EliminarAsync(int id);
}