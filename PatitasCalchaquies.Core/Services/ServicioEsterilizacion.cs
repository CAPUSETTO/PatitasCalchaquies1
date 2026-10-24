using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioEsterilizacion
{
    private readonly IRepositorioEsterilizacion _repositorio;

    public ServicioEsterilizacion(IRepositorioEsterilizacion repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Esterilizacion>> ObtenerTodasAsync()
    {
        return await _repositorio.ObtenerTodasAsync();
    }

    public async Task<Esterilizacion?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(Esterilizacion esterilizacion)
    {
        if (esterilizacion == null)
            throw new ArgumentNullException(nameof(esterilizacion));

        if (esterilizacion.IdAnimal <= 0)
            throw new ArgumentException("Debe seleccionar un animal válido.");

        if (esterilizacion.IdVeterinario <= 0)
            throw new ArgumentException("Debe seleccionar un veterinario válido.");

        if (string.IsNullOrWhiteSpace(esterilizacion.Tipo))
            throw new ArgumentException("El tipo de esterilización es obligatorio.");

        return await _repositorio.CrearAsync(esterilizacion);
    }

    public async Task<bool> ActualizarAsync(Esterilizacion esterilizacion)
    {
        if (esterilizacion == null)
            throw new ArgumentNullException(nameof(esterilizacion));

        if (esterilizacion.IdEsterilizacion <= 0)
            throw new ArgumentException("El ID de la esterilización no es válido.");

        if (esterilizacion.IdAnimal <= 0)
            throw new ArgumentException("Debe seleccionar un animal válido.");

        if (esterilizacion.IdVeterinario <= 0)
            throw new ArgumentException("Debe seleccionar un veterinario válido.");

        if (string.IsNullOrWhiteSpace(esterilizacion.Tipo))
            throw new ArgumentException("El tipo de esterilización es obligatorio.");

        var existente = await _repositorio.ObtenerPorIdAsync(esterilizacion.IdEsterilizacion);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró la esterilización con ID {esterilizacion.IdEsterilizacion}.");

        return await _repositorio.ActualizarAsync(esterilizacion);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró la esterilización con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}