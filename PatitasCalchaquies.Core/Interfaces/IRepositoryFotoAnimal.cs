using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioFotoAnimal
{
    Task<IEnumerable<FotoAnimal>> ObtenerTodasAsync();
    Task<FotoAnimal?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(FotoAnimal foto);
    Task<bool> ActualizarAsync(FotoAnimal foto);
    Task<bool> EliminarAsync(int id);
}