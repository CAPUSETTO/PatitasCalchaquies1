using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioUsuarioRol
{
    private readonly IRepositorioUsuarioRol _repositorio;

    public ServicioUsuarioRol(IRepositorioUsuarioRol repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<UsuarioRol>> ObtenerTodosAsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }

    public async Task<UsuarioRol?> ObtenerPorClaveAsync(int idUsuario, int idRol)
    {
        if (idUsuario <= 0 || idRol <= 0)
            throw new ArgumentException("Los identificadores de usuario y rol deben ser mayores a 0.");

        return await _repositorio.ObtenerPorClaveAsync(idUsuario, idRol);
    }

    public async Task<IEnumerable<UsuarioRol>> ObtenerPorUsuarioAsync(int idUsuario)
    {
        if (idUsuario <= 0)
            throw new ArgumentException("El ID de usuario debe ser mayor a 0.");

        return await _repositorio.ObtenerPorUsuarioAsync(idUsuario);
    }

    public async Task<bool> CrearAsync(UsuarioRol entidad)
    {
        if (entidad == null)
            throw new ArgumentNullException(nameof(entidad));

        if (entidad.IdUsuario <= 0)
            throw new ArgumentException("Debe seleccionar un usuario válido.");

        if (entidad.IdRol <= 0)
            throw new ArgumentException("Debe seleccionar un rol válido.");

        var existente = await _repositorio.ObtenerPorClaveAsync(entidad.IdUsuario, entidad.IdRol);
        if (existente != null)
            throw new InvalidOperationException("El usuario ya tiene asignado este rol.");

        return await _repositorio.CrearAsync(entidad);
    }

    public async Task<bool> EliminarAsync(int idUsuario, int idRol)
    {
        if (idUsuario <= 0 || idRol <= 0)
            throw new ArgumentException("Los identificadores de usuario y rol deben ser mayores a 0.");

        var existente = await _repositorio.ObtenerPorClaveAsync(idUsuario, idRol);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró la asignación del rol {idRol} al usuario {idUsuario}.");

        return await _repositorio.EliminarAsync(idUsuario, idRol);
    }
}