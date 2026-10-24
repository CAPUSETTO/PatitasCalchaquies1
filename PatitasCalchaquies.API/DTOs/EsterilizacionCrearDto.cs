namespace PatitasCalchaquies.API.DTOs;

public class EsterilizacionCrearDto
{
    public int IdAnimal { get; set; }
    public int IdVeterinario { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public string Tipo { get; set; } = string.Empty;
    public string? Observacion { get; set; }
}