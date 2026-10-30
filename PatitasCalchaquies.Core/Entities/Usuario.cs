using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("Usuarios")]
public class Usuario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdUsuario")]
    public int IdUsuario { get; set; }

    [Column("user")]
    [MaxLength(50, ErrorMessage = "El nombre de usuario no puede superar los 50 caracteres.")]
    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    public required string User { get; set; }

    [Column("password_hash")]
    [MaxLength(255, ErrorMessage = "El hash de la contraseña no puede superar los 255 caracteres.")]
    [Required(ErrorMessage = "El hash de la contraseña es obligatorio.")]
    public required string PasswordHash { get; set; }

    [Column("activo")]
    [Required(ErrorMessage = "El estado activo es obligatorio.")]
    public bool Activo { get; set; }

    [Column("fecha_creacion")]
    [Required(ErrorMessage = "La fecha de creación es obligatoria.")]
    public DateTime FechaCreacion { get; set; }

    [Column("ultimo_acceso")]
    public DateTime? UltimoAcceso { get; set; }

    [Column("nombre")]
    [MaxLength(50)]
    public string? Nombre { get; set; }

    [Column("apellido")]
    [MaxLength(50)]
    public string? Apellido { get; set; }

    [Column("etiqueta_area")]
    [MaxLength(100)]
    public string? EtiquetaArea { get; set; }

    [Column("id_veterinario")]
    public int? IdVeterinario { get; set; }

    // Propiedades proyectadas desde el JOIN con Roles y Veterinarios
    [NotMapped]
    public int? IdRol { get; set; }

    [NotMapped]
    public string? RolNombre { get; set; }

    [NotMapped]
    public string? RolDescripcion { get; set; }

    [NotMapped]
    public string? VeterinarioMatricula { get; set; }

    [NotMapped]
    public string? VeterinarioNombre { get; set; }

    [NotMapped]
    public string? VeterinarioApellido { get; set; }

    [NotMapped]
    public string? VeterinarioTelefono { get; set; }
}
