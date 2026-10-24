using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioPlanAlimentacion
{
    Task<IEnumerable<PlanAlimentacion>> ObtenerTodosAsync();
    Task<PlanAlimentacion?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(PlanAlimentacion plan);
    Task<bool> ActualizarAsync(PlanAlimentacion plan);
    Task<bool> EliminarAsync(int id);
}