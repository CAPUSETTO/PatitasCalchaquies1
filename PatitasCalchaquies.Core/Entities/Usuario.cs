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
}