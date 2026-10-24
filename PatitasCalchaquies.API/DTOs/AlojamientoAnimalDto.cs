namespace PatitasCalchaquies.API.DTOs;

public class AlojamientoAnimalDto
{
    public int IdAlojamiento { get; set; }
    public int IdAnimal { get; set; }
    public int IdSector { get; set; }
    public DateTime FechaIngreso { get; set; }
    public DateTime? FechaSalida { get; set; }
    public string? Observacion { get; set; }
}