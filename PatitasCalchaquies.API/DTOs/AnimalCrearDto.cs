namespace PatitasCalchaquies.API.DTOs;

public class AnimalCrearDto
{
    public string Nombre { get; set; } = string.Empty;
    public string CodigoIdentificacion { get; set; } = string.Empty;
    public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;
    public bool Castrado { get; set; }
    public int IdEspecie { get; set; }
    public int? IdRaza { get; set; }
    public int? IdSexo { get; set; }
    public int? IdEstado { get; set; }
}