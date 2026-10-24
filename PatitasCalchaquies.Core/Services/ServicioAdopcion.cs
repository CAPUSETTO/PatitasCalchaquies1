using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioAdopcion
{
    private readonly IRepositorioAdopcion _repositorio;

    public ServicioAdopcion(IRepositorioAdopcion repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Adopcion>> ObtenerTodasAsync()
    {
        return await _repositorio.ObtenerTodasAsync();
    }

    public async Task<Adopcion?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(Adopcion adopcion)
    {
        if (adopcion == null)
            throw new ArgumentNullException(nameof(adopcion));

        if (adopcion.IdSolicitudAdopcion <= 0)
            throw new ArgumentException("Debe asociarse a una solicitud de adopción válida.");

        return await _repositorio.CrearAsync(adopcion);
    }

    public async Task<bool> ActualizarAsync(Adopcion adopcion)
    {
        if (adopcion == null)
            throw new ArgumentNullException(nameof(adopcion));

        if (adopcion.IdAdopcion <= 0)
            throw new ArgumentException("El ID de la adopción no es válido.");

        var existente = await _repositorio.ObtenerPorIdAsync(adopcion.IdAdopcion);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró la adopción con ID {adopcion.IdAdopcion}.");

        return await _repositorio.ActualizarAsync(adopcion);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró la adopción con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}