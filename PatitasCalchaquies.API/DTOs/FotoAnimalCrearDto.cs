namespace PatitasCalchaquies.API.DTOs;

public class FotoAnimalCrearDto
{
    public int IdAnimal { get; set; }
    public string Ruta { get; set; } = string.Empty;
    public bool EsPrincipal { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
}