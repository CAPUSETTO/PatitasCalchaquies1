namespace PatitasCalchaquies.API.DTOs;

public class AlimentacionCrearDto
{
    public int IdAnimal { get; set; }
    public DateTime FechaHora { get; set; } = DateTime.Now;
    public string TipoAlimento { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public string? Observacion { get; set; }
}