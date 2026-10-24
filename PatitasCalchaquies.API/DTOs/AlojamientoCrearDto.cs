namespace PatitasCalchaquies.API.DTOs;

public class AlojamientoAnimalCrearDto
{
    public int IdAnimal { get; set; }
    public int IdSector { get; set; }
    public DateTime FechaIngreso { get; set; } = DateTime.Now;
    public DateTime? FechaSalida { get; set; }
    public string? Observacion { get; set; }
}