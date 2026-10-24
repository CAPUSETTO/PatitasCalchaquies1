using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioTratamientoMedicamento
{
    Task<IEnumerable<TratamientoMedicamento>> ObtenerTodosAsync();
    Task<TratamientoMedicamento?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(TratamientoMedicamento entidad);
    Task<bool> ActualizarAsync(TratamientoMedicamento entidad);
    Task<bool> EliminarAsync(int id);
}