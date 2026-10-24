using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioPredio
{
    private readonly IRepositorioPredio _repositorio;

    public ServicioPredio(IRepositorioPredio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Predio>> ObtenerTodosAsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }

    public async Task<Predio?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(Predio predio)
    {
        if (predio == null)
            throw new ArgumentNullException(nameof(predio));

        if (string.IsNullOrWhiteSpace(predio.Nombre))
            throw new ArgumentException("El nombre del predio es obligatorio.");

        return await _repositorio.CrearAsync(predio);
    }

    public async Task<bool> ActualizarAsync(Predio predio)
    {
        if (predio == null)
            throw new ArgumentNullException(nameof(predio));

        if (predio.IdPredio <= 0)
            throw new ArgumentException("El ID del predio no es válido.");

        if (string.IsNullOrWhiteSpace(predio.Nombre))
            throw new ArgumentException("El nombre del predio es obligatorio.");

        var existente = await _repositorio.ObtenerPorIdAsync(predio.IdPredio);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el predio con ID {predio.IdPredio}.");

        return await _repositorio.ActualizarAsync(predio);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el predio con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}