namespace PatitasCalchaquies.API.DTOs;

public class PlanAlimentacionActualizarDto
{
    public int IdPlan { get; set; }
    public int IdAnimal { get; set; }
    public string TipoAlimento { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public string Frecuencia { get; set; } = string.Empty;
    public string? Observacion { get; set; }
    public DateTime FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
}