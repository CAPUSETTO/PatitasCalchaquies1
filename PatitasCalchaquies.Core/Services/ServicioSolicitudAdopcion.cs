using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioSolicitudAdopcion
{
    private readonly IRepositorioSolicitudAdopcion _repositorio;

    public ServicioSolicitudAdopcion(IRepositorioSolicitudAdopcion repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<SolicitudAdopcion>> ObtenerTodasAsync()
    {
        return await _repositorio.ObtenerTodasAsync();
    }

    public async Task<SolicitudAdopcion?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(SolicitudAdopcion solicitud)
    {
        if (solicitud == null)
            throw new ArgumentNullException(nameof(solicitud));

        if (solicitud.IdPersona <= 0)
            throw new ArgumentException("Debe seleccionar una persona válida.");

        if (solicitud.IdAnimal <= 0)
            throw new ArgumentException("Debe seleccionar un animal válido.");

        if (string.IsNullOrWhiteSpace(solicitud.Estado))
            throw new ArgumentException("El estado de la solicitud es obligatorio.");

        return await _repositorio.CrearAsync(solicitud);
    }

    public async Task<bool> ActualizarAsync(SolicitudAdopcion solicitud)
    {
        if (solicitud == null)
            throw new ArgumentNullException(nameof(solicitud));

        if (solicitud.IdSolicitudAdopcion <= 0)
            throw new ArgumentException("El ID de la solicitud no es válido.");

        if (solicitud.IdPersona <= 0)
            throw new ArgumentException("Debe seleccionar una persona válida.");

        if (solicitud.IdAnimal <= 0)
            throw new ArgumentException("Debe seleccionar un animal válido.");

        if (string.IsNullOrWhiteSpace(solicitud.Estado))
            throw new ArgumentException("El estado de la solicitud es obligatorio.");

        var existente = await _repositorio.ObtenerPorIdAsync(solicitud.IdSolicitudAdopcion);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró la solicitud de adopción con ID {solicitud.IdSolicitudAdopcion}.");

        return await _repositorio.ActualizarAsync(solicitud);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró la solicitud de adopción con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}