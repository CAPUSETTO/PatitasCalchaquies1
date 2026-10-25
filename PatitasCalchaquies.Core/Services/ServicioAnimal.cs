using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Services;

public class ServicioAnimal
{
    readonly IRepositoryAnimal _repositorio;

    public ServicioAnimal(IRepositoryAnimal repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Animal>> ObtenerTodosAsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }

    public async Task<Animal> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El id no existe en el registro");
        }
        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(Animal animal)
    {
        if (animal == null)
            throw new ArgumentNullException(nameof(animal));

        if (string.IsNullOrWhiteSpace(animal.Nombre))
            throw new ArgumentException("El nombre del animal es obligatorio.");

        if (string.IsNullOrWhiteSpace(animal.CodigoIdentificacion))
            throw new ArgumentException("El código de identificación es obligatorio.");

        if (animal.IdEspecie <= 0)
            throw new ArgumentException("Debe asignarse una especie válida.");

        int id = await _repositorio.CrearAsync(animal);
        if (id <= 0)
            throw new InvalidOperationException("No se pudo registrar el animal. Verifique el código de identificación y los catálogos asociados.");

        return id;
    }

    public async Task<bool> ActualizarAsync(Animal animal)
    {
        if (animal == null)
        {
            throw new ArgumentNullException(nameof(animal));
        }

        if (animal.IdAnimal <= 0)
        {
            throw new ArgumentException("El id del animal es obligatorio.");
        }
        
        var existente = await _repositorio.ObtenerPorIdAsync(animal.IdAnimal);
        if (existente == null)
        {
            throw new KeyNotFoundException($"No se encontro el animal con ID {animal.IdAnimal}");
        }
        
        return await _repositorio.ActualizarAsync(animal);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El id del animal no existe.");
        }
        
        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
        {
            throw new KeyNotFoundException($"No se encontro el animal con ID {id}");
        }
        
        return await _repositorio.EliminarAsync(id);
    }
}
