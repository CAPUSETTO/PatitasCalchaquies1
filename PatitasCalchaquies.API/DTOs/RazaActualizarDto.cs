namespace PatitasCalchaquies.API.DTOs;

public class RazaActualizarDto
{
    public int IdRaza { get; set; }
    public int IdEspecie { get; set; }
    public required string Nombre { get; set; }
}