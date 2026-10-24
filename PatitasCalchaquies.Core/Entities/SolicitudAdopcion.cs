using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("Solicitud_Adopcion")]
public class SolicitudAdopcion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdSolicitudAdopcion")]
    public int IdSolicitudAdopcion { get; set; }

    [Column("id_persona")]
    [Required(ErrorMessage = "La persona es obligatoria.")]
    public int IdPersona { get; set; }

    [Column("id_animal")]
    [Required(ErrorMessage = "El animal es obligatorio.")]
    public int IdAnimal { get; set; }

    [Column("fecha")]
    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime Fecha { get; set; }

    [Column("estado")]
    [MaxLength(50, ErrorMessage = "El estado no puede superar los 50 caracteres.")]
    [Required(ErrorMessage = "El estado de la solicitud es obligatorio.")]
    public required string Estado { get; set; }

    [Column("observacion")]
    public string? Observacion { get; set; }
}