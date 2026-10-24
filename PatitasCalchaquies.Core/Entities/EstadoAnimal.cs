using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("Estado_Animal")]
public class EstadoAnimal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdEstadoAnimal")]
    public int IdEstadoAnimal { get; set; }

    [Column("nombre")]
    [MaxLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    [Required(ErrorMessage = "El nombre del estado es obligatorio.")]
    public required string Nombre { get; set; }
}