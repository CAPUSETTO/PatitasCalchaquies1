using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("Tratamientos")]
public class Tratamiento
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdTratamiento")]
    public int IdTratamiento { get; set; }

    [Column("id_atencion_veterinaria")]
    [Required(ErrorMessage = "La atención veterinaria es obligatoria.")]
    public int IdAtencionVeterinaria { get; set; }

    [Column("descripcion")]
    [MaxLength(255, ErrorMessage = "La descripción no puede superar los 255 caracteres.")]
    [Required(ErrorMessage = "La descripción del tratamiento es obligatoria.")]
    public required string Descripcion { get; set; }

    [Column("fecha_inicio")]
    [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
    public DateTime FechaInicio { get; set; }

    [Column("fecha_fin")]
    public DateTime? FechaFin { get; set; }

    [Column("estado")]
    [MaxLength(50, ErrorMessage = "El estado no puede superar los 50 caracteres.")]
    [Required(ErrorMessage = "El estado del tratamiento es obligatorio.")]
    public required string Estado { get; set; }

    [Column("observacion")]
    public string? Observacion { get; set; }
}