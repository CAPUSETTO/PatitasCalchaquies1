using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("Sexo")]
public class Sexo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdSexo")]
    public int IdSexo { get; set; }

    [Column("nombre")]
    [MaxLength(20, ErrorMessage = "El nombre no puede superar los 20 caracteres.")]
    [Required(ErrorMessage = "El nombre del sexo es obligatorio.")]
    public required string Nombre { get; set; }
}