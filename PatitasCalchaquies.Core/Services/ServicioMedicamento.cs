using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioMedicamento
{
    private readonly IRepositoryMedicamento _repositoryMedicamento;

    public ServicioMedicamento(IRepositoryMedicamento repositoryMedicamento)
    {
        _repositoryMedicamento = repositoryMedicamento;
    }

    public async Task<IEnumerable<Medicamento>> Allsync()
    {
        return await _repositoryMedicamento.ObtenerTodosAsync();
    }

    public async Task<Medicamento> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El id debe ser mayor a 0");
        }
        return await _repositoryMedicamento.ObtenerPorIdAsync(id);
    }

    public async Task<IEnumerable<Medicamento>> BuscarAsync(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto)){}
        {
            return await _repositoryMedicamento.ObtenerTodosAsync();
        }
        return await _repositoryMedicamento.BuscarAsync(texto);
    }

    public async Task<int> CrearMedicamentoAsync(Medicamento medicamento)
    {
        if (string.IsNullOrWhiteSpace(medicamento.Nombre))
        {
            throw new ArgumentException("El nombre del medicamento es obligatorio");
        }
        
        return await _repositoryMedicamento.CrearAsync(medicamento);
    }

    public async Task<bool> ActualizarAsync(Medicamento medicamento)
    {
        if (medicamento.IdMedicamento <= 0)
        {
            throw new ArgumentException("El id del medicamento debe ser menor a 0");
        }

        var existente = await _repositoryMedicamento.ObtenerPorIdAsync(medicamento.IdMedicamento);

        if (existente == null)
        {
            return false;
        }
        return await _repositoryMedicamento.ActualizarAsync(medicamento);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El id del medicamento debe ser menor a 0");
        }
        
        return await _repositoryMedicamento.EliminarAsync(id);
    }
    
}