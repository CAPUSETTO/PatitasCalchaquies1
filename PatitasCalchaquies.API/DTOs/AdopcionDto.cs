namespace PatitasCalchaquies.API.DTOs;

public class AdopcionDto
{
    public int IdAdopcion { get; set; }
    public int IdSolicitudAdopcion { get; set; }
    public DateTime Fecha { get; set; }
    public string? Observacion { get; set; }
}