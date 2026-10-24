using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("Ingreso_Animal")]
public class IngresoAnimal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdIngresoAnimal")]
    public int IdIngresoAnimal { get; set; }

    [Column("id_animal")]
    [Required(ErrorMessage = "El animal es obligatorio.")]
    public int IdAnimal { get; set; }

    [Column("fecha")]
    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime Fecha { get; set; }

    [Column("motivo")]
    [MaxLength(255, ErrorMessage = "El motivo no puede superar los 255 caracteres.")]
    [Required(ErrorMessage = "El motivo del ingreso es obligatorio.")]
    public required string Motivo { get; set; }

    [Column("tipo_ingreso")]
    [MaxLength(50, ErrorMessage = "El tipo de ingreso no puede superar los 50 caracteres.")]
    [Required(ErrorMessage = "El tipo de ingreso es obligatorio.")]
    public required string TipoIngreso { get; set; }

    [Column("lugar_origen")]
    [MaxLength(150, ErrorMessage = "El lugar de origen no puede superar los 150 caracteres.")]
    public string? LugarOrigen { get; set; }

    [Column("observacion")]
    public string? Observacion { get; set; }
}