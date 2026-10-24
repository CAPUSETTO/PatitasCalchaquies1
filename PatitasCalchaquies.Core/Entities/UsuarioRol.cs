using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("Usuario_Rol")]
public class UsuarioRol
{
    [Column("id_usuario")]
    [Required(ErrorMessage = "El usuario es obligatorio.")]
    public int IdUsuario { get; set; }

    [Column("id_rol")]
    [Required(ErrorMessage = "El rol es obligatorio.")]
    public int IdRol { get; set; }
}