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

        int id = await _repositorio.CrearRazaAsync(raza);
        if (id <= 0)
            throw new InvalidOperationException("No se pudo guardar la raza en la base de datos.");

        return id;
    }
    
    public async Task<bool> ActualizarAsync(Raza raza)
    {
        if (raza == null)
            throw new ArgumentNullException(nameof(raza));

        if (raza.IdRaza <= 0)
            throw new ArgumentException("El ID de la raza no es válido.");

        if (raza.IdEspecie <= 0)
            throw new ArgumentException("Debe seleccionar una especie válida.");

        if (string.IsNullOrWhiteSpace(raza.Nombre))
            throw new ArgumentException("El nombre de la raza es obligatorio.");

        var existente = await _repositorio.ObtenerPorEspecieAsync(raza.IdRaza);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró la raza con ID {raza.IdRaza}.");

        return await _repositorio.ActualizarAsync(raza);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorEspecieAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró la raza con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}
