using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioTratamientoMedicamento
{
    private readonly IRepositorioTratamientoMedicamento _repositorio;

    public ServicioTratamientoMedicamento(IRepositorioTratamientoMedicamento repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<TratamientoMedicamento>> ObtenerTodosAsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }

    public async Task<TratamientoMedicamento?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(TratamientoMedicamento entidad)
    {
        if (entidad == null)
            throw new ArgumentNullException(nameof(entidad));

        if (entidad.IdMedicamento <= 0)
            throw new ArgumentException("Debe seleccionar un medicamento válido.");

        if (entidad.IdTratamiento <= 0)
            throw new ArgumentException("Debe seleccionar un tratamiento válido.");

        if (string.IsNullOrWhiteSpace(entidad.Dosis))
            throw new ArgumentException("La dosis es obligatoria.");

        if (string.IsNullOrWhiteSpace(entidad.Frecuencia))
            throw new ArgumentException("La frecuencia es obligatoria.");

        if (string.IsNullOrWhiteSpace(entidad.Duracion))
            throw new ArgumentException("La duración es obligatoria.");

        return await _repositorio.CrearAsync(entidad);
    }

    public async Task<bool> ActualizarAsync(TratamientoMedicamento entidad)
    {
        if (entidad == null)
            throw new ArgumentNullException(nameof(entidad));

        if (entidad.IdTratamientoMedico <= 0)
            throw new ArgumentException("El ID de la prescripción médica no es válido.");

        if (entidad.IdMedicamento <= 0)
            throw new ArgumentException("Debe seleccionar un medicamento válido.");

        if (entidad.IdTratamiento <= 0)
            throw new ArgumentException("Debe seleccionar un tratamiento válido.");

        if (string.IsNullOrWhiteSpace(entidad.Dosis))
            throw new ArgumentException("La dosis es obligatoria.");

        if (string.IsNullOrWhiteSpace(entidad.Frecuencia))
            throw new ArgumentException("La frecuencia es obligatoria.");

        if (string.IsNullOrWhiteSpace(entidad.Duracion))
            throw new ArgumentException("La duración es obligatoria.");

        var existente = await _repositorio.ObtenerPorIdAsync(entidad.IdTratamientoMedico);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el registro con ID {entidad.IdTratamientoMedico}.");

        return await _repositorio.ActualizarAsync(entidad);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el registro con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}