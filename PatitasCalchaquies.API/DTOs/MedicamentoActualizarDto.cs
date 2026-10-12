namespace PatitasCalchaquies.API.DTOs;

public class MedicamentoActualizarDto
{
    public int idMedicamento { get; set; }
    public string nombre { get; set; } = string.Empty;
    public string descripcion { get; set; } = string.Empty;
}