namespace PatitasCalchaquies.API.DTOs;

public class PredioDto
{
    public int IdPredio { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Direccion { get; set; }
    public string? Descripcion { get; set; }
}