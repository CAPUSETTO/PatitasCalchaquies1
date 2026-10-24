namespace PatitasCalchaquies.API.DTOs;

public class VacunacionActualizarDto
{
    public int IdVacunacion { get; set; }
    public int IdAnimal { get; set; }
    public int IdVacuna { get; set; }
    public DateTime Fecha { get; set; }
    public DateTime? ProximaFecha { get; set; }
    public string? Observacion { get; set; }
}