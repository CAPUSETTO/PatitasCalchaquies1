using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioAtencionVeterinaria
{
    private readonly IRepositorioAtencionVeterinaria _repositorio;

    public ServicioAtencionVeterinaria(IRepositorioAtencionVeterinaria repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<AtencionVeterinaria>> ObtenerTodasAsync()
    {
        return await _repositorio.ObtenerTodasAsync();
    }

    public async Task<AtencionVeterinaria?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(AtencionVeterinaria atencion)
    {
        if (atencion == null)
            throw new ArgumentNullException(nameof(atencion));

        if (atencion.IdAnimal <= 0)
            throw new ArgumentException("Debe seleccionar un animal válido.");

        if (atencion.IdVeterinario <= 0)
            throw new ArgumentException("Debe seleccionar un veterinario válido.");

        if (string.IsNullOrWhiteSpace(atencion.Motivo))
            throw new ArgumentException("El motivo de la atención es obligatorio.");

        return await _repositorio.CrearAsync(atencion);
    }

    public async Task<bool> ActualizarAsync(AtencionVeterinaria atencion)
    {
        if (atencion == null)
            throw new ArgumentNullException(nameof(atencion));

        if (atencion.IdAtencionVeterinaria <= 0)
            throw new ArgumentException("El ID de la atención veterinaria no es válido.");

        if (atencion.IdAnimal <= 0)
            throw new ArgumentException("Debe seleccionar un animal válido.");

        if (atencion.IdVeterinario <= 0)
            throw new ArgumentException("Debe seleccionar un veterinario válido.");

        if (string.IsNullOrWhiteSpace(atencion.Motivo))
            throw new ArgumentException("El motivo de la atención es obligatorio.");

        var existente = await _repositorio.ObtenerPorIdAsync(atencion.IdAtencionVeterinaria);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró la atención veterinaria con ID {atencion.IdAtencionVeterinaria}.");

        return await _repositorio.ActualizarAsync(atencion);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró la atención veterinaria con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}