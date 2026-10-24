using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("esterilizacion")]
public class Esterilizacion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdEsterilizacion")]
    public int IdEsterilizacion { get; set; }

    [Column("id_animal")]
    [Required(ErrorMessage = "El animal es obligatorio.")]
    public int IdAnimal { get; set; }

    [Column("id_veterinario")]
    [Required(ErrorMessage = "El veterinario es obligatorio.")]
    public int IdVeterinario { get; set; }

    [Column("fecha")]
    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime Fecha { get; set; }

    [Column("tipo")]
    [MaxLength(50, ErrorMessage = "El tipo no puede superar los 50 caracteres.")]
    [Required(ErrorMessage = "El tipo de esterilización es obligatorio.")]
    public required string Tipo { get; set; }

    [Column("observacion")]
    public string? Observacion { get; set; }
}