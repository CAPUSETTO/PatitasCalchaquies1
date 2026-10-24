namespace PatitasCalchaquies.API.DTOs;

public class DevolucionAdopcionCrearDto
{
    public int IdAdopcion { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public string Motivo { get; set; } = string.Empty;
    public string? Observacion { get; set; }
}