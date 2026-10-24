namespace PatitasCalchaquies.API.DTOs;

public class PlanAlimentacionCrearDto
{
    public int IdAnimal { get; set; }
    public string TipoAlimento { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public string Frecuencia { get; set; } = string.Empty;
    public string? Observacion { get; set; }
    public DateTime FechaDesde { get; set; } = DateTime.Now;
    public DateTime? FechaHasta { get; set; }
}