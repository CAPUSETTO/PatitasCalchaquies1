using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("Roles")]
public class Rol
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdRol")]
    public int IdRol { get; set; }

    [Column("nombre")]
    [MaxLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
    public required string Nombre { get; set; }

    [Column("descripcion")]
    [MaxLength(255, ErrorMessage = "La descripción no puede superar los 255 caracteres.")]
    public string? Descripcion { get; set; }
}