namespace PatitasCalchaquies.API.DTOs;

public class TratamientoMedicamentoDto
{
    public int IdTratamientoMedico { get; set; }
    public int IdMedicamento { get; set; }
    public int IdTratamiento { get; set; }
    public string Dosis { get; set; } = string.Empty;
    public string Frecuencia { get; set; } = string.Empty;
    public string Duracion { get; set; } = string.Empty;
    public string? Observacion { get; set; }
}