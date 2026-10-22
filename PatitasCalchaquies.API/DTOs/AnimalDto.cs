namespace PatitasCalchaquies.API.DTOs;

public class AnimalDto
{
    public int IdAnimal { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string CodigoIdentificacion { get; set; } = string.Empty;
    public DateTime FechaIngreso { get; set; }
    public bool Castrado { get; set; }
    public int IdEspecie { get; set; }
    public int? IdRaza { get; set; }
    public int? IdSexo { get; set; }
    public int? IdEstado { get; set; }
}