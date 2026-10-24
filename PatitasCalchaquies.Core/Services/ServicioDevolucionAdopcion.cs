using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioDevolucionAdopcion
{
    private readonly IRepositorioDevolucionAdopcion _repositorio;

    public ServicioDevolucionAdopcion(IRepositorioDevolucionAdopcion repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<DevolucionAdopcion>> ObtenerTodasAsync()
    {
        return await _repositorio.ObtenerTodasAsync();
    }

    public async Task<DevolucionAdopcion?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(DevolucionAdopcion devolucion)
    {
        if (devolucion == null)
            throw new ArgumentNullException(nameof(devolucion));

        if (devolucion.IdAdopcion <= 0)
            throw new ArgumentException("Debe seleccionar una adopción válida.");

        if (string.IsNullOrWhiteSpace(devolucion.Motivo))
            throw new ArgumentException("El motivo de la devolución es obligatorio.");

        return await _repositorio.CrearAsync(devolucion);
    }

    public async Task<bool> ActualizarAsync(DevolucionAdopcion devolucion)
    {
        if (devolucion == null)
            throw new ArgumentNullException(nameof(devolucion));

        if (devolucion.IdDevolucionAdopcion <= 0)
            throw new ArgumentException("El ID de la devolución no es válido.");

        if (devolucion.IdAdopcion <= 0)
            throw new ArgumentException("Debe seleccionar una adopción válida.");

        if (string.IsNullOrWhiteSpace(devolucion.Motivo))
            throw new ArgumentException("El motivo de la devolución es obligatorio.");

        var existente = await _repositorio.ObtenerPorIdAsync(devolucion.IdDevolucionAdopcion);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró la devolución con ID {devolucion.IdDevolucionAdopcion}.");

        return await _repositorio.ActualizarAsync(devolucion);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró la devolución con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}