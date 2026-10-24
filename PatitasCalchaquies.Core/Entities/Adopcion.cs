using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatitasCalchaquies.Core.Entities;

[Table("adopciones")]
public class Adopcion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdAdopcion")]
    public int IdAdopcion { get; set; }

    [Column("id_solicitud_adopcion")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe vincularse a una solicitud de adopción válida")]
    public int IdSolicitudAdopcion { get; set; } // Foreign Key

    [Column("fecha")]
    public DateTime Fecha { get; set; }

    [Column("observacion")]
    [MaxLength(500, ErrorMessage = "La observación no puede superar los 500 caracteres")]
    public string? Observacion { get; set; }
}