using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioDevolucionAdopcion
{
    Task<IEnumerable<DevolucionAdopcion>> ObtenerTodasAsync();
    Task<DevolucionAdopcion?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(DevolucionAdopcion devolucion);
    Task<bool> ActualizarAsync(DevolucionAdopcion devolucion);
    Task<bool> EliminarAsync(int id);
}