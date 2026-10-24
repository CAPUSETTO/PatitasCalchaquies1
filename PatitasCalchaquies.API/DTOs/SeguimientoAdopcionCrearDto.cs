namespace PatitasCalchaquies.API.DTOs;

public class SeguimientoAdopcionCrearDto
{
    public int IdAdopcion { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public string Tipo { get; set; } = string.Empty;
    public string Resultado { get; set; } = string.Empty;
    public string? Observacion { get; set; }
}