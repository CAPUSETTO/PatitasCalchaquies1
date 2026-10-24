using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("Devolucio_Adopcion")]
public class DevolucionAdopcion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdDevolucionAdopcion")]
    public int IdDevolucionAdopcion { get; set; }

    [Column("id_adopcion")]
    [Required(ErrorMessage = "La adopción es obligatoria.")]
    public int IdAdopcion { get; set; }

    [Column("fecha")]
    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime Fecha { get; set; }

    [Column("motivo")]
    [MaxLength(255, ErrorMessage = "El motivo no puede superar los 255 caracteres.")]
    [Required(ErrorMessage = "El motivo es obligatorio.")]
    public required string Motivo { get; set; }

    [Column("observacion")]
    public string? Observacion { get; set; }
}