namespace PatitasCalchaquies.API.DTOs;

public class UsuarioDto
{
    public int IdUsuario { get; set; }
    public string User { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? UltimoAcceso { get; set; }
}