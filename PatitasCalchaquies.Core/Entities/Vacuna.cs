using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    [Table("Vacunas")]
    public class Vacuna
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdVacuna")]
        public int IdVacuna { get; set; }
        [Column("Nombre")]
        [MaxLength(100)]
        public required string Nombre { get; set; }
        [Column("Descripcion")]
        [MaxLength(100)]
        public string? Descripcion { get; set; }
    }
}
