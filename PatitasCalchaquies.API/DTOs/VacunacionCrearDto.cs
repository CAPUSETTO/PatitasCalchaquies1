namespace PatitasCalchaquies.API.DTOs;

public class VacunacionCrearDto
{
    public int IdAnimal { get; set; }
    public int IdVacuna { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public DateTime? ProximaFecha { get; set; }
    public string? Observacion { get; set; }
}