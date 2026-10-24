namespace PatitasCalchaquies.API.DTOs;

public class AlimentacionDto
{
    public int IdAlimentacion { get; set; }
    public int IdAnimal { get; set; }
    public DateTime FechaHora { get; set; }
    public string TipoAlimento { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public string? Observacion { get; set; }
}