using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioSexo
{
    Task<IEnumerable<Sexo>> ObtenerTodosAsync();
    Task<Sexo?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Sexo sexo);
    Task<bool> ActualizarAsync(Sexo sexo);
    Task<bool> EliminarAsync(int id);
}