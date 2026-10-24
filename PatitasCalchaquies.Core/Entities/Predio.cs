using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("Predio")]
public class Predio
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdPredio")]
    public int IdPredio { get; set; }

    [Column("nombre")]
    [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    [Required(ErrorMessage = "El nombre del predio es obligatorio.")]
    public required string Nombre { get; set; }

    [Column("direccion")]
    [MaxLength(150, ErrorMessage = "La dirección no puede superar los 150 caracteres.")]
    public string? Direccion { get; set; }

    [Column("descripcion")]
    [MaxLength(255, ErrorMessage = "La descripción no puede superar los 255 caracteres.")]
    public string? Descripcion { get; set; }
}