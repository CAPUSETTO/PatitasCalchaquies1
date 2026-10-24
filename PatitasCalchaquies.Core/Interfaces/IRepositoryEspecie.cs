using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioEspecie
{
    Task<IEnumerable<Especie>> ObtenerTodasAsync();
    Task<Especie?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Especie especie);
    Task<bool> ActualizarAsync(Especie especie);
    Task<bool> EliminarAsync(int id);
}