using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("Atencion_Veterinario")]
public class AtencionVeterinaria
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdAtencionVeterinaria")]
    public int IdAtencionVeterinaria { get; set; }

    [Column("id_animal")]
    [Required(ErrorMessage = "El animal es obligatorio.")]
    public int IdAnimal { get; set; }

    [Column("id_veterinario")]
    [Required(ErrorMessage = "El veterinario es obligatorio.")]
    public int IdVeterinario { get; set; }

    [Column("fecha")]
    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime Fecha { get; set; }

    [Column("motivo")]
    [MaxLength(255, ErrorMessage = "El motivo no puede superar los 255 caracteres.")]
    [Required(ErrorMessage = "El motivo es obligatorio.")]
    public required string Motivo { get; set; }
}