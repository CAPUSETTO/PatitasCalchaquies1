namespace PatitasCalchaquies.API.DTOs;

public class EsterilizacionDto
{
    public int IdEsterilizacion { get; set; }
    public int IdAnimal { get; set; }
    public int IdVeterinario { get; set; }
    public DateTime Fecha { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string? Observacion { get; set; }
}