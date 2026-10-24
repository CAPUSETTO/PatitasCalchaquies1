using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("Sector")]
public class Sector
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdSector")]
    public int IdSector { get; set; }

    [Column("nombre")]
    [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    [Required(ErrorMessage = "El nombre del sector es obligatorio.")]
    public required string Nombre { get; set; }

    [Column("capacidad")]
    public int? Capacidad { get; set; }

    [Column("descripcion")]
    [MaxLength(255, ErrorMessage = "La descripción no puede superar los 255 caracteres.")]
    public string? Descripcion { get; set; }

    [Column("id_predio")]
    [Required(ErrorMessage = "El predio es obligatorio.")]
    public int IdPredio { get; set; }
}