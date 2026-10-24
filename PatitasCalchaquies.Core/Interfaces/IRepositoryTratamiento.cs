using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioTratamiento
{
    Task<IEnumerable<Tratamiento>> ObtenerTodosAsync();
    Task<Tratamiento?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Tratamiento tratamiento);
    Task<bool> ActualizarAsync(Tratamiento tratamiento);
    Task<bool> EliminarAsync(int id);
}