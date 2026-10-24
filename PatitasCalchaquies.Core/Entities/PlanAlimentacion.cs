using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("Plan_Alimentacion")]
public class PlanAlimentacion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdPlan")]
    public int IdPlan { get; set; }

    [Column("id_animal")]
    [Required(ErrorMessage = "El animal es obligatorio.")]
    public int IdAnimal { get; set; }

    [Column("tipo_alimento")]
    [MaxLength(100, ErrorMessage = "El tipo de alimento no puede superar los 100 caracteres.")]
    [Required(ErrorMessage = "El tipo de alimento es obligatorio.")]
    public required string TipoAlimento { get; set; }

    [Column("cantidad")]
    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    public decimal Cantidad { get; set; }

    [Column("frecuencia")]
    [MaxLength(50, ErrorMessage = "La frecuencia no puede superar los 50 caracteres.")]
    [Required(ErrorMessage = "La frecuencia es obligatoria.")]
    public required string Frecuencia { get; set; }

    [Column("observacion")]
    public string? Observacion { get; set; }

    [Column("fecha_desde")]
    [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
    public DateTime FechaDesde { get; set; }

    [Column("fecha_hasta")]
    public DateTime? FechaHasta { get; set; }
}