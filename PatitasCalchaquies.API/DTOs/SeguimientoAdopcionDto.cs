namespace PatitasCalchaquies.API.DTOs;

public class SeguimientoAdopcionDto
{
    public int IdSeguimientoAdopcion { get; set; }
    public int IdAdopcion { get; set; }
    public DateTime Fecha { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Resultado { get; set; } = string.Empty;
    public string? Observacion { get; set; }
}