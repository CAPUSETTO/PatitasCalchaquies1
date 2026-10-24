using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioVacunacion
{
    private readonly IRepositorioVacunacion _repositorio;

    public ServicioVacunacion(IRepositorioVacunacion repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Vacunacion>> ObtenerTodasAsync()
    {
        return await _repositorio.ObtenerTodasAsync();
    }

    public async Task<Vacunacion?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(Vacunacion vacunacion)
    {
        if (vacunacion == null)
            throw new ArgumentNullException(nameof(vacunacion));

        if (vacunacion.IdAnimal <= 0)
            throw new ArgumentException("Debe seleccionar un animal válido.");

        if (vacunacion.IdVacuna <= 0)
            throw new ArgumentException("Debe seleccionar una vacuna válida.");

        if (vacunacion.ProximaFecha.HasValue && vacunacion.ProximaFecha.Value < vacunacion.Fecha)
            throw new ArgumentException("La próxima fecha no puede ser anterior a la fecha de aplicación.");

        return await _repositorio.CrearAsync(vacunacion);
    }

    public async Task<bool> ActualizarAsync(Vacunacion vacunacion)
    {
        if (vacunacion == null)
            throw new ArgumentNullException(nameof(vacunacion));

        if (vacunacion.IdVacunacion <= 0)
            throw new ArgumentException("El ID de la vacunación no es válido.");

        if (vacunacion.IdAnimal <= 0)
            throw new ArgumentException("Debe seleccionar un animal válido.");

        if (vacunacion.IdVacuna <= 0)
            throw new ArgumentException("Debe seleccionar una vacuna válida.");

        if (vacunacion.ProximaFecha.HasValue && vacunacion.ProximaFecha.Value < vacunacion.Fecha)
            throw new ArgumentException("La próxima fecha no puede ser anterior a la fecha de aplicación.");

        var existente = await _repositorio.ObtenerPorIdAsync(vacunacion.IdVacunacion);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el registro de vacunación con ID {vacunacion.IdVacunacion}.");

        return await _repositorio.ActualizarAsync(vacunacion);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el registro de vacunación con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}