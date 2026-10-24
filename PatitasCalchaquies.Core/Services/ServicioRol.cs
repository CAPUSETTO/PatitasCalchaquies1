using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioRol
{
    private readonly IRepositorioRol _repositorio;

    public ServicioRol(IRepositorioRol repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Rol>> ObtenerTodosAsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }

    public async Task<Rol?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(Rol rol)
    {
        if (rol == null)
            throw new ArgumentNullException(nameof(rol));

        if (string.IsNullOrWhiteSpace(rol.Nombre))
            throw new ArgumentException("El nombre del rol es obligatorio.");

        return await _repositorio.CrearAsync(rol);
    }

    public async Task<bool> ActualizarAsync(Rol rol)
    {
        if (rol == null)
            throw new ArgumentNullException(nameof(rol));

        if (rol.IdRol <= 0)
            throw new ArgumentException("El ID del rol no es válido.");

        if (string.IsNullOrWhiteSpace(rol.Nombre))
            throw new ArgumentException("El nombre del rol es obligatorio.");

        var existente = await _repositorio.ObtenerPorIdAsync(rol.IdRol);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el rol con ID {rol.IdRol}.");

        return await _repositorio.ActualizarAsync(rol);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el rol con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}