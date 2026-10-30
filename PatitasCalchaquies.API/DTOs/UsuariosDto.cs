namespace PatitasCalchaquies.API.DTOs;

public class UsuarioDto
{
    public int IdUsuario { get; set; }
    public string User { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? UltimoAcceso { get; set; }
}

public class DatosProfesionalesVeterinarioDto
{
    public int? IdVeterinario { get; set; }
    public string Matricula { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
}

public class OperadorSistemaDto
{
    public int IdUsuario { get; set; }
    public string User { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string EtiquetaArea { get; set; } = string.Empty;
    public string ResumenPermisos { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? UltimoAcceso { get; set; }
    public RolDto Rol { get; set; } = new();
    public DatosProfesionalesVeterinarioDto? Veterinario { get; set; }
}

public class OperadorCrearDto
{
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int IdRol { get; set; } = 2;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string? EtiquetaArea { get; set; }
    public bool? Activo { get; set; } = true;
    public string? Matricula { get; set; }
    public string? Telefono { get; set; }
}

public class OperadorActualizarDto
{
    public int IdUsuario { get; set; }
    public string User { get; set; } = string.Empty;
    public string? Password { get; set; }
    public int IdRol { get; set; } = 2;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string? EtiquetaArea { get; set; }
    public bool Activo { get; set; } = true;
    public string? Matricula { get; set; }
    public string? Telefono { get; set; }
}

public class CambiarEstadoOperadorDto
{
    public bool Activo { get; set; }
}
