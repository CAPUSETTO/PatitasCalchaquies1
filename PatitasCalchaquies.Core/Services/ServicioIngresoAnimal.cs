using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioIngresoAnimal
{
    private readonly IRepositorioIngresoAnimal _repositorio;

    public ServicioIngresoAnimal(IRepositorioIngresoAnimal repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<IngresoAnimal>> ObtenerTodosAsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }

    public async Task<IngresoAnimal?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(IngresoAnimal ingreso)
    {
        if (ingreso == null)
            throw new ArgumentNullException(nameof(ingreso));

        if (ingreso.IdAnimal <= 0)
            throw new ArgumentException("Debe seleccionar un animal válido.");

        if (string.IsNullOrWhiteSpace(ingreso.Motivo))
            throw new ArgumentException("El motivo del ingreso es obligatorio.");

        if (string.IsNullOrWhiteSpace(ingreso.TipoIngreso))
            throw new ArgumentException("El tipo de ingreso es obligatorio.");

        return await _repositorio.CrearAsync(ingreso);
    }

    public async Task<bool> ActualizarAsync(IngresoAnimal ingreso)
    {
        if (ingreso == null)
            throw new ArgumentNullException(nameof(ingreso));

        if (ingreso.IdIngresoAnimal <= 0)
            throw new ArgumentException("El ID del ingreso no es válido.");

        if (ingreso.IdAnimal <= 0)
            throw new ArgumentException("Debe seleccionar un animal válido.");

        if (string.IsNullOrWhiteSpace(ingreso.Motivo))
            throw new ArgumentException("El motivo del ingreso es obligatorio.");

        if (string.IsNullOrWhiteSpace(ingreso.TipoIngreso))
            throw new ArgumentException("El tipo de ingreso es obligatorio.");

        var existente = await _repositorio.ObtenerPorIdAsync(ingreso.IdIngresoAnimal);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el ingreso con ID {ingreso.IdIngresoAnimal}.");

        return await _repositorio.ActualizarAsync(ingreso);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el ingreso con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}