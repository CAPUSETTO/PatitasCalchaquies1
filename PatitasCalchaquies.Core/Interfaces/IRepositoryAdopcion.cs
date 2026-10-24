using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioAdopcion
{
    Task<IEnumerable<Adopcion>> ObtenerTodasAsync();
    Task<Adopcion?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Adopcion adopcion);
    Task<bool> ActualizarAsync(Adopcion adopcion);
    Task<bool> EliminarAsync(int id);
}