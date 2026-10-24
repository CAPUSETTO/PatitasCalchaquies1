namespace PatitasCalchaquies.API.DTOs;

public class AtencionVeterinarioActualizarDto
{
    public int IdAtencionVeterinaria { get; set; }
    public int IdAnimal { get; set; }
    public int IdVeterinario { get; set; }
    public DateTime Fecha { get; set; }
    public string Motivo { get; set; } = string.Empty;
}