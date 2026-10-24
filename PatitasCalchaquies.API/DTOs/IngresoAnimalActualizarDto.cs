namespace PatitasCalchaquies.API.DTOs;

public class IngresoAnimalActualizarDto
{
    public int IdIngresoAnimal { get; set; }
    public int IdAnimal { get; set; }
    public DateTime Fecha { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string TipoIngreso { get; set; } = string.Empty;
    public string? LugarOrigen { get; set; }
    public string? Observacion { get; set; }
}