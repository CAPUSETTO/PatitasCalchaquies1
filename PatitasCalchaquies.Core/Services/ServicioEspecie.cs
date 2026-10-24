using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioEspecie
{
    private readonly IRepositorioEspecie _repositorio;

    public ServicioEspecie(IRepositorioEspecie repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Especie>> ObtenerTodasAsync()
    {
        return await _repositorio.ObtenerTodasAsync();
    }

    public async Task<Especie?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(Especie especie)
    {
        if (especie == null)
            throw new ArgumentNullException(nameof(especie));

        if (string.IsNullOrWhiteSpace(especie.Nombre))
            throw new ArgumentException("El nombre de la especie es obligatorio.");

        return await _repositorio.CrearAsync(especie);
    }

    public async Task<bool> ActualizarAsync(Especie especie)
    {
        if (especie == null)
            throw new ArgumentNullException(nameof(especie));

        if (especie.IdEspecie <= 0)
            throw new ArgumentException("El ID de la especie no es válido.");

        if (string.IsNullOrWhiteSpace(especie.Nombre))
            throw new ArgumentException("El nombre de la especie es obligatorio.");

        var existente = await _repositorio.ObtenerPorIdAsync(especie.IdEspecie);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró la especie con ID {especie.IdEspecie}.");

        return await _repositorio.ActualizarAsync(especie);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró la especie con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}