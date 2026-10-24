using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("Alojamiento_Animal")]
public class AlojamientoAnimal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdAlojamiento")]
    public int IdAlojamiento { get; set; }

    [Column("id_animal")]
    [Required(ErrorMessage = "El animal es obligatorio.")]
    public int IdAnimal { get; set; }

    [Column("id_sector")]
    [Required(ErrorMessage = "El sector es obligatorio.")]
    public int IdSector { get; set; }

    [Column("fecha_ingreso")]
    [Required(ErrorMessage = "La fecha de ingreso es obligatoria.")]
    public DateTime FechaIngreso { get; set; }

    [Column("fecha_salida")]
    public DateTime? FechaSalida { get; set; }

    [Column("observacion")]
    public string? Observacion { get; set; }
}