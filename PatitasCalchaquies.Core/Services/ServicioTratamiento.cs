using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioTratamiento
{
    private readonly IRepositorioTratamiento _repositorio;

    public ServicioTratamiento(IRepositorioTratamiento repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Tratamiento>> ObtenerTodosAsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }

    public async Task<Tratamiento?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(Tratamiento tratamiento)
    {
        if (tratamiento == null)
            throw new ArgumentNullException(nameof(tratamiento));

        if (tratamiento.IdAtencionVeterinaria <= 0)
            throw new ArgumentException("Debe asociarse a una atención veterinaria válida.");

        if (string.IsNullOrWhiteSpace(tratamiento.Descripcion))
            throw new ArgumentException("La descripción del tratamiento es obligatoria.");

        if (string.IsNullOrWhiteSpace(tratamiento.Estado))
            throw new ArgumentException("El estado del tratamiento es obligatorio.");

        if (tratamiento.FechaFin.HasValue && tratamiento.FechaFin.Value < tratamiento.FechaInicio)
            throw new ArgumentException("La fecha de fin no puede ser anterior a la fecha de inicio.");

        return await _repositorio.CrearAsync(tratamiento);
    }

    public async Task<bool> ActualizarAsync(Tratamiento tratamiento)
    {
        if (tratamiento == null)
            throw new ArgumentNullException(nameof(tratamiento));

        if (tratamiento.IdTratamiento <= 0)
            throw new ArgumentException("El ID del tratamiento no es válido.");

        if (tratamiento.IdAtencionVeterinaria <= 0)
            throw new ArgumentException("Debe asociarse a una atención veterinaria válida.");

        if (string.IsNullOrWhiteSpace(tratamiento.Descripcion))
            throw new ArgumentException("La descripción del tratamiento es obligatoria.");

        if (string.IsNullOrWhiteSpace(tratamiento.Estado))
            throw new ArgumentException("El estado del tratamiento es obligatorio.");

        if (tratamiento.FechaFin.HasValue && tratamiento.FechaFin.Value < tratamiento.FechaInicio)
            throw new ArgumentException("La fecha de fin no puede ser anterior a la fecha de inicio.");

        var existente = await _repositorio.ObtenerPorIdAsync(tratamiento.IdTratamiento);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el tratamiento con ID {tratamiento.IdTratamiento}.");

        return await _repositorio.ActualizarAsync(tratamiento);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el tratamiento con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}