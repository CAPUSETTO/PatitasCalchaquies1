using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public class IRepositoryMedicamento
{
    Task<IEnumerable<Medicamento>> ObtenerTodosAsync();
    Task<Medicamento> ObtenerPorIdAsync(int id);;
    Task<IEnumerable<Medicamento> BuscarAsync(string texto);
    Task<int> CrearAsync (Medicamento medicamento);
    Task<bool> ActualizarAsync (Medicamento medicamento);
    Task<bool> EliminarAsync(int id);
}