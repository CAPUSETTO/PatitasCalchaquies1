using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("Tratamiento_Medicamento")]
public class TratamientoMedicamento
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdTratamientoMedico")]
    public int IdTratamientoMedico { get; set; }

    [Column("id_medicamento")]
    [Required(ErrorMessage = "El medicamento es obligatorio.")]
    public int IdMedicamento { get; set; }

    [Column("id_tratamiento")]
    [Required(ErrorMessage = "El tratamiento es obligatorio.")]
    public int IdTratamiento { get; set; }

    [Column("dosis")]
    [MaxLength(100, ErrorMessage = "La dosis no puede superar los 100 caracteres.")]
    [Required(ErrorMessage = "La dosis es obligatoria.")]
    public required string Dosis { get; set; }

    [Column("frecuencia")]
    [MaxLength(100, ErrorMessage = "La frecuencia no puede superar los 100 caracteres.")]
    [Required(ErrorMessage = "La frecuencia es obligatoria.")]
    public required string Frecuencia { get; set; }

    [Column("duracion")]
    [MaxLength(100, ErrorMessage = "La duración no puede superar los 100 caracteres.")]
    [Required(ErrorMessage = "La duración es obligatoria.")]
    public required string Duracion { get; set; }

    [Column("observacion")]
    public string? Observacion { get; set; }
}