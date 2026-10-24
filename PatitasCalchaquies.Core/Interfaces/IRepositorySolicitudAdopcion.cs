using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositorioSolicitudAdopcion
{
    Task<IEnumerable<SolicitudAdopcion>> ObtenerTodasAsync();
    Task<SolicitudAdopcion?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(SolicitudAdopcion solicitud);
    Task<bool> ActualizarAsync(SolicitudAdopcion solicitud);
    Task<bool> EliminarAsync(int id);
}