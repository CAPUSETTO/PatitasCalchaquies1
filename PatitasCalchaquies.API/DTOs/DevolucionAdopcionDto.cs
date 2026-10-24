namespace PatitasCalchaquies.API.DTOs;

public class DevolucionAdopcionDto
{
    public int IdDevolucionAdopcion { get; set; }
    public int IdAdopcion { get; set; }
    public DateTime Fecha { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string? Observacion { get; set; }
}