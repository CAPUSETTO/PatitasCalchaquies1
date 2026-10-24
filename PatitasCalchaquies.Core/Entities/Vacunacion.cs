using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("Vacunacion")]
public class Vacunacion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdVacunacion")]
    public int IdVacunacion { get; set; }

    [Column("id_animal")]
    [Required(ErrorMessage = "El animal es obligatorio.")]
    public int IdAnimal { get; set; }

    [Column("id_vacuna")]
    [Required(ErrorMessage = "La vacuna es obligatoria.")]
    public int IdVacuna { get; set; }

    [Column("fecha")]
    [Required(ErrorMessage = "La fecha de aplicación es obligatoria.")]
    public DateTime Fecha { get; set; }

    [Column("proxima_fecha")]
    public DateTime? ProximaFecha { get; set; }

    [Column("observacion")]
    public string? Observacion { get; set; }
}