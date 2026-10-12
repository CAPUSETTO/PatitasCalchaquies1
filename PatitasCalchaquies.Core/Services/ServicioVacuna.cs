using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioVacuna
{
    private readonly IRepositoryVacuna _repositoryVacuna;

    public ServicioVacuna(IRepositoryVacuna repositoryVacuna)
    {
        _repositoryVacuna = repositoryVacuna;
    }

    public async Task<IEnumerable<Vacuna>> Allsync()
    {
        return await _repositoryVacuna.ObtenerTodosAsync();
    }

    public async Task<Vacuna> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El id debe ser mayor a 0");
        }
        return await _repositoryVacuna.ObtenerPorIdAsync(id);
    }

    public async Task<IEnumerable<Vacuna>> BuscarAsync(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto)){}
        {
            return await _repositoryVacuna.ObtenerTodosAsync();
        }
        return await _repositoryVacuna.BuscarAsync(texto);
    }

    public async Task<int> CrearVacunaAsync(Vacuna vacuna)
    {
        if (string.IsNullOrWhiteSpace(vacuna.Nombre))
        {
            throw new ArgumentException("El nombre del medicamento es obligatorio");
        }
        
        return await _repositoryVacuna.CrearAsync(vacuna);
    }

    public async Task<bool> ActualizarAsync(Vacuna vacuna)
    {
        if (vacuna.IdVacuna <= 0)
        {
            throw new ArgumentException("El id del medicamento debe ser menor a 0");
        }

        var existente = await _repositoryVacuna.ObtenerPorIdAsync(vacuna.IdVacuna);

        if (existente == null)
        {
            return false;
        }
        return await _repositoryVacuna.ActualizarAsync(vacuna);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El id del medicamento debe ser menor a 0");
        }
        
        return await _repositoryVacuna.EliminarAsync(id);
    }

}