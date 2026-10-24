namespace PatitasCalchaquies.API.DTOs;

public class RolActualizarDto
{
    public int IdRol { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}