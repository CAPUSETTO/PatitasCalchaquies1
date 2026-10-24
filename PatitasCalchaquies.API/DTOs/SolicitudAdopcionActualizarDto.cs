namespace PatitasCalchaquies.API.DTOs;

public class SolicitudAdopcionActualizarDto
{
    public int IdSolicitudAdopcion { get; set; }
    public int IdPersona { get; set; }
    public int IdAnimal { get; set; }
    public DateTime Fecha { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? Observacion { get; set; }
}