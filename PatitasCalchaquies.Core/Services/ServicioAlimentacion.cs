using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioAlimentacion
{
    private readonly IRepositorioAlimentacion _repositorio;

    public ServicioAlimentacion(IRepositorioAlimentacion repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Alimentacion>> ObtenerTodasAsync()
    {
        return await _repositorio.ObtenerTodasAsync();
    }

    public async Task<Alimentacion?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(Alimentacion alimentacion)
    {
        if (alimentacion == null)
            throw new ArgumentNullException(nameof(alimentacion));

        if (alimentacion.IdAnimal <= 0)
            throw new ArgumentException("Debe asociarse a un animal válido.");

        if (string.IsNullOrWhiteSpace(alimentacion.TipoAlimento))
            throw new ArgumentException("El tipo de alimento es obligatorio.");

        if (alimentacion.Cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a 0.");

        return await _repositorio.CrearAsync(alimentacion);
    }

    public async Task<bool> ActualizarAsync(Alimentacion alimentacion)
    {
        if (alimentacion == null)
            throw new ArgumentNullException(nameof(alimentacion));

        if (alimentacion.IdAlimentacion <= 0)
            throw new ArgumentException("El ID de la alimentación no es válido.");

        if (alimentacion.IdAnimal <= 0)
            throw new ArgumentException("Debe asociarse a un animal válido.");

        if (alimentacion.Cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(alimentacion.IdAlimentacion);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el registro de alimentación con ID {alimentacion.IdAlimentacion}.");

        return await _repositorio.ActualizarAsync(alimentacion);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el registro de alimentación con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}