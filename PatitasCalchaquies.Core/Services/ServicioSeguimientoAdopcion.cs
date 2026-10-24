using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioSeguimientoAdopcion
{
    private readonly IRepositorioSeguimientoAdopcion _repositorio;

    public ServicioSeguimientoAdopcion(IRepositorioSeguimientoAdopcion repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<SeguimientoAdopcion>> ObtenerTodosAsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }

    public async Task<SeguimientoAdopcion?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(SeguimientoAdopcion seguimiento)
    {
        if (seguimiento == null)
            throw new ArgumentNullException(nameof(seguimiento));

        if (seguimiento.IdAdopcion <= 0)
            throw new ArgumentException("Debe seleccionar una adopción válida.");

        if (string.IsNullOrWhiteSpace(seguimiento.Tipo))
            throw new ArgumentException("El tipo de seguimiento es obligatorio.");

        if (string.IsNullOrWhiteSpace(seguimiento.Resultado))
            throw new ArgumentException("El resultado del seguimiento es obligatorio.");

        return await _repositorio.CrearAsync(seguimiento);
    }

    public async Task<bool> ActualizarAsync(SeguimientoAdopcion seguimiento)
    {
        if (seguimiento == null)
            throw new ArgumentNullException(nameof(seguimiento));

        if (seguimiento.IdSeguimientoAdopcion <= 0)
            throw new ArgumentException("El ID del seguimiento no es válido.");

        if (seguimiento.IdAdopcion <= 0)
            throw new ArgumentException("Debe seleccionar una adopción válida.");

        if (string.IsNullOrWhiteSpace(seguimiento.Tipo))
            throw new ArgumentException("El tipo de seguimiento es obligatorio.");

        if (string.IsNullOrWhiteSpace(seguimiento.Resultado))
            throw new ArgumentException("El resultado del seguimiento es obligatorio.");

        var existente = await _repositorio.ObtenerPorIdAsync(seguimiento.IdSeguimientoAdopcion);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el seguimiento con ID {seguimiento.IdSeguimientoAdopcion}.");

        return await _repositorio.ActualizarAsync(seguimiento);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el seguimiento con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}