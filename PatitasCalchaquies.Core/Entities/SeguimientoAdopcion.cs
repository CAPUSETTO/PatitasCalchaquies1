using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("Seguimiento_Adopcion")]
public class SeguimientoAdopcion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdSeguimientoAdopcion")]
    public int IdSeguimientoAdopcion { get; set; }

    [Column("id_adopcion")]
    [Required(ErrorMessage = "La adopción es obligatoria.")]
    public int IdAdopcion { get; set; }

    [Column("fecha")]
    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime Fecha { get; set; }

    [Column("tipo")]
    [MaxLength(50, ErrorMessage = "El tipo no puede superar los 50 caracteres.")]
    [Required(ErrorMessage = "El tipo de seguimiento es obligatorio.")]
    public required string Tipo { get; set; }

    [Column("resultado")]
    [MaxLength(100, ErrorMessage = "El resultado no puede superar los 100 caracteres.")]
    [Required(ErrorMessage = "El resultado es obligatorio.")]
    public required string Resultado { get; set; }

    [Column("observacion")]
    public string? Observacion { get; set; }
}