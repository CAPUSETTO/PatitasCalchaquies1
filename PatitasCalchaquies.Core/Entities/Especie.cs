using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("especies")]
public class Especie
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdEspecie")]
    public int IdEspecie { get; set; }

    [Column("nombre")]
    [MaxLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    [Required(ErrorMessage = "El nombre de la especie es obligatorio.")]
    public required string Nombre { get; set; }
}