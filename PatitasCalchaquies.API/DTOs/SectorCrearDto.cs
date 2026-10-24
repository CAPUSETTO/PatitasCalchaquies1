namespace PatitasCalchaquies.API.DTOs;

public class SectorCrearDto
{
    public string Nombre { get; set; } = string.Empty;
    public int? Capacidad { get; set; }
    public string? Descripcion { get; set; }
    public int IdPredio { get; set; }
}