using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioFotoAnimal
{
    private readonly IRepositorioFotoAnimal _repositorio;

    public ServicioFotoAnimal(IRepositorioFotoAnimal repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<FotoAnimal>> ObtenerTodasAsync()
    {
        return await _repositorio.ObtenerTodasAsync();
    }

    public async Task<FotoAnimal?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(FotoAnimal foto)
    {
        if (foto == null)
            throw new ArgumentNullException(nameof(foto));

        if (foto.IdAnimal <= 0)
            throw new ArgumentException("Debe seleccionar un animal válido.");

        if (string.IsNullOrWhiteSpace(foto.Ruta))
            throw new ArgumentException("La ruta de la foto es obligatoria.");

        return await _repositorio.CrearAsync(foto);
    }

    public async Task<bool> ActualizarAsync(FotoAnimal foto)
    {
        if (foto == null)
            throw new ArgumentNullException(nameof(foto));

        if (foto.IdFotoAnimal <= 0)
            throw new ArgumentException("El ID de la foto no es válido.");

        if (foto.IdAnimal <= 0)
            throw new ArgumentException("Debe seleccionar un animal válido.");

        if (string.IsNullOrWhiteSpace(foto.Ruta))
            throw new ArgumentException("La ruta de la foto es obligatoria.");

        var existente = await _repositorio.ObtenerPorIdAsync(foto.IdFotoAnimal);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró la foto con ID {foto.IdFotoAnimal}.");

        return await _repositorio.ActualizarAsync(foto);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró la foto con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}