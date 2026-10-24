using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("Veterinarios")]
public class Veterinario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdVeterinario")]
    public int IdVeterinario { get; set; }

    [Column("matricula")]
    [MaxLength(50, ErrorMessage = "La matrícula no puede superar los 50 caracteres.")]
    [Required(ErrorMessage = "La matrícula es obligatoria.")]
    public required string Matricula { get; set; }

    [Column("nombre")]
    [MaxLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public required string Nombre { get; set; }

    [Column("apellido")]
    [MaxLength(50, ErrorMessage = "El apellido no puede superar los 50 caracteres.")]
    [Required(ErrorMessage = "El apellido es obligatorio.")]
    public required string Apellido { get; set; }

    [Column("telefono")]
    [MaxLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres.")]
    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    public required string Telefono { get; set; }
}