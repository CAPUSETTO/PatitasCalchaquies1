using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioUsuario
{
    private readonly IRepositorioUsuario _repositorio;

    public ServicioUsuario(IRepositorioUsuario repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }

    public async Task<Usuario?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(Usuario usuario)
    {
        if (usuario == null)
            throw new ArgumentNullException(nameof(usuario));

        if (string.IsNullOrWhiteSpace(usuario.User))
            throw new ArgumentException("El nombre de usuario es obligatorio.");

        if (string.IsNullOrWhiteSpace(usuario.PasswordHash))
            throw new ArgumentException("La contraseña hash es obligatoria.");

        return await _repositorio.CrearAsync(usuario);
    }

    public async Task<bool> ActualizarAsync(Usuario usuario)
    {
        if (usuario == null)
            throw new ArgumentNullException(nameof(usuario));

        if (usuario.IdUsuario <= 0)
            throw new ArgumentException("El ID del usuario no es válido.");

        if (string.IsNullOrWhiteSpace(usuario.User))
            throw new ArgumentException("El nombre de usuario es obligatorio.");

        if (string.IsNullOrWhiteSpace(usuario.PasswordHash))
            throw new ArgumentException("La contraseña hash es obligatoria.");

        var existente = await _repositorio.ObtenerPorIdAsync(usuario.IdUsuario);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el usuario con ID {usuario.IdUsuario}.");

        return await _repositorio.ActualizarAsync(usuario);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el usuario con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}