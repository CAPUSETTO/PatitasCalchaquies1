using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositoryRaza
{
    Task <IEnumerable<Raza>> ObtenerTodasAsync();
    Task<IEnumerable<Raza>> ObtenerPorEspecieAsync(int idEspecie);
    Task<int> CrearRazaAsync(Raza raza);
}