using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioRaza
{
    private readonly IRepositoryRaza _repositorio;

    public ServicioRaza(IRepositoryRaza repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Raza>> AllAsync()
    {
        return await _repositorio.ObtenerTodasAsync();
    }

    public async Task<IEnumerable<Raza>> GetByEspecieAsync(int idEspecie)
    {
        if (idEspecie <= 0)
            throw new ArgumentException("El ID de especie debe ser mayor a 0");

        return await _repositorio.ObtenerPorEspecieAsync(idEspecie);
    }

    public async Task<int> CrearAsync(Raza raza)
    {
        if (raza == null)
            throw new ArgumentNullException(nameof(raza));

        if (raza.IdEspecie <= 0)
            throw new ArgumentException("Debe indicar una especie válida");

        if (string.IsNullOrWhiteSpace(raza.Nombre))
            throw new ArgumentException("El nombre de la raza es obligatorio");

        return await _repositorio.CrearRazaAsync(raza);
    }
}