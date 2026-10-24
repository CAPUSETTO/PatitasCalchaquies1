namespace PatitasCalchaquies.API.DTOs;

public class TratamientoCrearDto
{
    public int IdAtencionVeterinaria { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; } = DateTime.Now;
    public DateTime? FechaFin { get; set; }
    public string Estado { get; set; } = "En curso";
    public string? Observacion { get; set; }
}