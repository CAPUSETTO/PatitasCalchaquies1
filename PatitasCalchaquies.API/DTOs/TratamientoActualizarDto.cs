namespace PatitasCalchaquies.API.DTOs;

public class TratamientoActualizarDto
{
    public int IdTratamiento { get; set; }
    public int IdAtencionVeterinaria { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? Observacion { get; set; }
}