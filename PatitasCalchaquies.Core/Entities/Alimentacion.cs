using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("alimentaciones")]
public class Alimentacion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdAlimentacion")]
    public int IdAlimentacion { get; set; }

    [Column("id_animal")]
    [Required(ErrorMessage = "El animal es obligatorio.")]
    public int IdAnimal { get; set; }

    [Column("fecha_hora")]
    [Required(ErrorMessage = "La fecha y hora son obligatorias.")]
    public DateTime FechaHora { get; set; }

    [Column("tipo_alimento")]
    [MaxLength(100, ErrorMessage = "El tipo de alimento no puede superar los 100 caracteres.")]
    [Required(ErrorMessage = "El tipo de alimento es obligatorio.")]
    public required string TipoAlimento { get; set; }

    [Column("cantidad")]
    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    public decimal Cantidad { get; set; }

    [Column("observacion")]
    public string? Observacion { get; set; }
}
