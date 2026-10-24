namespace PatitasCalchaquies.API.DTOs;

public class AdopcionCrearDto
{
    public int IdSolicitudAdopcion { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string? Observacion { get; set; }
}