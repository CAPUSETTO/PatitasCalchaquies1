namespace PatitasCalchaquies.API.DTOs;

public class IngresoAnimalCrearDto
{
    public int IdAnimal { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public string Motivo { get; set; } = string.Empty;
    public string TipoIngreso { get; set; } = string.Empty;
    public string? LugarOrigen { get; set; }
    public string? Observacion { get; set; }
}