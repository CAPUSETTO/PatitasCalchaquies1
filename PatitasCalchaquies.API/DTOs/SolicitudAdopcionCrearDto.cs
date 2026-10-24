namespace PatitasCalchaquies.API.DTOs;

public class SolicitudAdopcionCrearDto
{
    public int IdPersona { get; set; }
    public int IdAnimal { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public string Estado { get; set; } = "Pendiente";
    public string? Observacion { get; set; }
}