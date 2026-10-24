namespace PatitasCalchaquies.API.DTOs;

public class UsuarioActualizarDto
{
    public int IdUsuario { get; set; }
    public string User { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public DateTime? UltimoAcceso { get; set; }
}