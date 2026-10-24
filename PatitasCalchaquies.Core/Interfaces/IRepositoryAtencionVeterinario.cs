using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioAtencionVeterinaria
{
    Task<IEnumerable<AtencionVeterinaria>> ObtenerTodasAsync();
    Task<AtencionVeterinaria?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(AtencionVeterinaria atencion);
    Task<bool> ActualizarAsync(AtencionVeterinaria atencion);
    Task<bool> EliminarAsync(int id);
}