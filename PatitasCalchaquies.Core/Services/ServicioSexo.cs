using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioSexo
{
    private readonly IRepositorioSexo _repositorio;

    public ServicioSexo(IRepositorioSexo repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Sexo>> ObtenerTodosAsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }

    public async Task<Sexo?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(Sexo sexo)
    {
        if (sexo == null)
            throw new ArgumentNullException(nameof(sexo));

        if (string.IsNullOrWhiteSpace(sexo.Nombre))
            throw new ArgumentException("El nombre del sexo es obligatorio.");

        return await _repositorio.CrearAsync(sexo);
    }

    public async Task<bool> ActualizarAsync(Sexo sexo)
    {
        if (sexo == null)
            throw new ArgumentNullException(nameof(sexo));

        if (sexo.IdSexo <= 0)
            throw new ArgumentException("El ID del sexo no es válido.");

        if (string.IsNullOrWhiteSpace(sexo.Nombre))
            throw new ArgumentException("El nombre del sexo es obligatorio.");

        var existente = await _repositorio.ObtenerPorIdAsync(sexo.IdSexo);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el sexo con ID {sexo.IdSexo}.");

        return await _repositorio.ActualizarAsync(sexo);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el sexo con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}