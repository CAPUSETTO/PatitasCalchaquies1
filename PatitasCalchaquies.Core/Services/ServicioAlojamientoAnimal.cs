using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioAlojamientoAnimal
{
    private readonly IRepositorioAlojamientoAnimal _repositorio;

    public ServicioAlojamientoAnimal(IRepositorioAlojamientoAnimal repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<AlojamientoAnimal>> ObtenerTodosAsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }

    public async Task<AlojamientoAnimal?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(AlojamientoAnimal alojamiento)
    {
        if (alojamiento == null)
            throw new ArgumentNullException(nameof(alojamiento));

        if (alojamiento.IdAnimal <= 0)
            throw new ArgumentException("Debe seleccionar un animal válido.");

        if (alojamiento.IdSector <= 0)
            throw new ArgumentException("Debe seleccionar un sector válido.");

        if (alojamiento.FechaSalida.HasValue && alojamiento.FechaSalida.Value < alojamiento.FechaIngreso)
            throw new ArgumentException("La fecha de salida no puede ser anterior a la fecha de ingreso.");

        return await _repositorio.CrearAsync(alojamiento);
    }

    public async Task<bool> ActualizarAsync(AlojamientoAnimal alojamiento)
    {
        if (alojamiento == null)
            throw new ArgumentNullException(nameof(alojamiento));

        if (alojamiento.IdAlojamiento <= 0)
            throw new ArgumentException("El ID del alojamiento no es válido.");

        if (alojamiento.IdAnimal <= 0)
            throw new ArgumentException("Debe seleccionar un animal válido.");

        if (alojamiento.IdSector <= 0)
            throw new ArgumentException("Debe seleccionar un sector válido.");

        if (alojamiento.FechaSalida.HasValue && alojamiento.FechaSalida.Value < alojamiento.FechaIngreso)
            throw new ArgumentException("La fecha de salida no puede ser anterior a la fecha de ingreso.");

        var existente = await _repositorio.ObtenerPorIdAsync(alojamiento.IdAlojamiento);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el alojamiento con ID {alojamiento.IdAlojamiento}.");

        return await _repositorio.ActualizarAsync(alojamiento);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el alojamiento con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}