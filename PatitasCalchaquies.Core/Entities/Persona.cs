using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class Persona
    {
        [Key]
        [DatabaseGenerated(databaseGeneratedOption:DatabaseGeneratedOption.Identity)]
        [Column("IdPersona")]
        public int IdPersona { get; set; }
        [Column("Dni")]
        [MaxLength(50)]
        public required string Dni { get; set; }
        [Column("nombre")]
        [MaxLength(100)]
        public required string Nombre { get; set; }
        [Column("apellido")]
        [MaxLength(100)]
        public required string Apellido { get; set; }
        [Column("telefono")]
        [MaxLength(20)]
        public required string Telefono { get; set; }
        [Column("email")]
        [MaxLength(100)]
        public string? Email { get; set; }
        [Column("direccion")]
        [MaxLength(100)]
        public required string Direccion { get; set; }
        [Column("fecha_alta")]
        public DateTime FechaAlta { get; set; }
        [Column("activo")]
        public bool Activo { get; set; }
    }
}
