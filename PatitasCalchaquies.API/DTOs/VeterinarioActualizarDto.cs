namespace PatitasCalchaquies.API.DTOs;

public class VeterinarioActualizarDto
{
    public int IdVeterinario { get; set; }
    public string Matricula { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
}