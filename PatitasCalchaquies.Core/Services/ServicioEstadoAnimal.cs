using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioEstadoAnimal
{
    private readonly IRepositorioEstadoAnimal _repositorio;

    public ServicioEstadoAnimal(IRepositorioEstadoAnimal repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<EstadoAnimal>> ObtenerTodosAsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }

    public async Task<EstadoAnimal?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(EstadoAnimal estado)
    {
        if (estado == null)
            throw new ArgumentNullException(nameof(estado));

        if (string.IsNullOrWhiteSpace(estado.Nombre))
            throw new ArgumentException("El nombre del estado es obligatorio.");

        return await _repositorio.CrearAsync(estado);
    }

    public async Task<bool> ActualizarAsync(EstadoAnimal estado)
    {
        if (estado == null)
            throw new ArgumentNullException(nameof(estado));

        if (estado.IdEstadoAnimal <= 0)
            throw new ArgumentException("El ID del estado no es válido.");

        if (string.IsNullOrWhiteSpace(estado.Nombre))
            throw new ArgumentException("El nombre del estado es obligatorio.");

        var existente = await _repositorio.ObtenerPorIdAsync(estado.IdEstadoAnimal);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el estado con ID {estado.IdEstadoAnimal}.");

        return await _repositorio.ActualizarAsync(estado);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el estado con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}