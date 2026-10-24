using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioSector
{
    private readonly IRepositorioSector _repositorio;

    public ServicioSector(IRepositorioSector repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Sector>> ObtenerTodosAsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }

    public async Task<Sector?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(Sector sector)
    {
        if (sector == null)
            throw new ArgumentNullException(nameof(sector));

        if (string.IsNullOrWhiteSpace(sector.Nombre))
            throw new ArgumentException("El nombre del sector es obligatorio.");

        if (sector.IdPredio <= 0)
            throw new ArgumentException("Debe seleccionar un predio válido.");

        if (sector.Capacidad.HasValue && sector.Capacidad.Value < 0)
            throw new ArgumentException("La capacidad no puede ser negativa.");

        return await _repositorio.CrearAsync(sector);
    }

    public async Task<bool> ActualizarAsync(Sector sector)
    {
        if (sector == null)
            throw new ArgumentNullException(nameof(sector));

        if (sector.IdSector <= 0)
            throw new ArgumentException("El ID del sector no es válido.");

        if (string.IsNullOrWhiteSpace(sector.Nombre))
            throw new ArgumentException("El nombre del sector es obligatorio.");

        if (sector.IdPredio <= 0)
            throw new ArgumentException("Debe seleccionar un predio válido.");

        if (sector.Capacidad.HasValue && sector.Capacidad.Value < 0)
            throw new ArgumentException("La capacidad no puede ser negativa.");

        var existente = await _repositorio.ObtenerPorIdAsync(sector.IdSector);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el sector con ID {sector.IdSector}.");

        return await _repositorio.ActualizarAsync(sector);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el sector con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}