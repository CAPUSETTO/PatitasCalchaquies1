using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("Foto_Animal")]
public class FotoAnimal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdFotoAnimal")]
    public int IdFotoAnimal { get; set; }

    [Column("id_animal")]
    [Required(ErrorMessage = "El animal es obligatorio.")]
    public int IdAnimal { get; set; }

    [Column("ruta")]
    [MaxLength(255, ErrorMessage = "La ruta no puede superar los 255 caracteres.")]
    [Required(ErrorMessage = "La ruta de la foto es obligatoria.")]
    public required string Ruta { get; set; }

    [Column("es_principal")]
    [Required(ErrorMessage = "Debe indicar si es la foto principal.")]
    public bool EsPrincipal { get; set; }

    [Column("fecha")]
    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime Fecha { get; set; }
}