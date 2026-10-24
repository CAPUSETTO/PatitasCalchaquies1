using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioVeterinario
{
    private readonly IRepositorioVeterinario _repositorio;

    public ServicioVeterinario(IRepositorioVeterinario repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Veterinario>> ObtenerTodosAsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }

    public async Task<Veterinario?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(Veterinario veterinario)
    {
        if (veterinario == null)
            throw new ArgumentNullException(nameof(veterinario));

        if (string.IsNullOrWhiteSpace(veterinario.Matricula))
            throw new ArgumentException("La matrícula es obligatoria.");

        if (string.IsNullOrWhiteSpace(veterinario.Nombre))
            throw new ArgumentException("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(veterinario.Apellido))
            throw new ArgumentException("El apellido es obligatorio.");

        if (string.IsNullOrWhiteSpace(veterinario.Telefono))
            throw new ArgumentException("El teléfono es obligatorio.");

        return await _repositorio.CrearAsync(veterinario);
    }

    public async Task<bool> ActualizarAsync(Veterinario veterinario)
    {
        if (veterinario == null)
            throw new ArgumentNullException(nameof(veterinario));

        if (veterinario.IdVeterinario <= 0)
            throw new ArgumentException("El ID del veterinario no es válido.");

        if (string.IsNullOrWhiteSpace(veterinario.Matricula))
            throw new ArgumentException("La matrícula es obligatoria.");

        if (string.IsNullOrWhiteSpace(veterinario.Nombre))
            throw new ArgumentException("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(veterinario.Apellido))
            throw new ArgumentException("El apellido es obligatorio.");

        if (string.IsNullOrWhiteSpace(veterinario.Telefono))
            throw new ArgumentException("El teléfono es obligatorio.");

        var existente = await _repositorio.ObtenerPorIdAsync(veterinario.IdVeterinario);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el veterinario con ID {veterinario.IdVeterinario}.");

        return await _repositorio.ActualizarAsync(veterinario);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el veterinario con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}