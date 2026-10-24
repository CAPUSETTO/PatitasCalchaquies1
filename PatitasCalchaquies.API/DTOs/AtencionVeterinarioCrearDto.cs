namespace PatitasCalchaquies.API.DTOs;

public class AtencionVeterinariaCrearDto
{
    public int IdAnimal { get; set; }
    public int IdVeterinario { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public string Motivo { get; set; } = string.Empty;
}