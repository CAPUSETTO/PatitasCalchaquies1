using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioPlanAlimentacion
{
    private readonly IRepositorioPlanAlimentacion _repositorio;

    public ServicioPlanAlimentacion(IRepositorioPlanAlimentacion repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<PlanAlimentacion>> ObtenerTodosAsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }

    public async Task<PlanAlimentacion?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(PlanAlimentacion plan)
    {
        if (plan == null)
            throw new ArgumentNullException(nameof(plan));

        if (plan.IdAnimal <= 0)
            throw new ArgumentException("Debe seleccionar un animal válido.");

        if (string.IsNullOrWhiteSpace(plan.TipoAlimento))
            throw new ArgumentException("El tipo de alimento es obligatorio.");

        if (plan.Cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a 0.");

        if (string.IsNullOrWhiteSpace(plan.Frecuencia))
            throw new ArgumentException("La frecuencia es obligatoria.");

        if (plan.FechaHasta.HasValue && plan.FechaHasta.Value < plan.FechaDesde)
            throw new ArgumentException("La fecha hasta no puede ser anterior a la fecha de inicio.");

        return await _repositorio.CrearAsync(plan);
    }

    public async Task<bool> ActualizarAsync(PlanAlimentacion plan)
    {
        if (plan == null)
            throw new ArgumentNullException(nameof(plan));

        if (plan.IdPlan <= 0)
            throw new ArgumentException("El ID del plan no es válido.");

        if (plan.IdAnimal <= 0)
            throw new ArgumentException("Debe seleccionar un animal válido.");

        if (string.IsNullOrWhiteSpace(plan.TipoAlimento))
            throw new ArgumentException("El tipo de alimento es obligatorio.");

        if (plan.Cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a 0.");

        if (string.IsNullOrWhiteSpace(plan.Frecuencia))
            throw new ArgumentException("La frecuencia es obligatoria.");

        if (plan.FechaHasta.HasValue && plan.FechaHasta.Value < plan.FechaDesde)
            throw new ArgumentException("La fecha hasta no puede ser anterior a la fecha de inicio.");

        var existente = await _repositorio.ObtenerPorIdAsync(plan.IdPlan);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el plan con ID {plan.IdPlan}.");

        return await _repositorio.ActualizarAsync(plan);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el plan con ID {id}.");

        return await _repositorio.EliminarAsync(id);
    }
}