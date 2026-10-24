namespace PatitasCalchaquies.API.DTOs;

public class AdopcionActualizarDto
{
    public int IdAdopcion { get; set; }
    public int IdSolicitudAdopcion { get; set; }
    public DateTime Fecha { get; set; }
    public string? Observacion { get; set; }
}