using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    [Table("animales")]
    public class Animal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdAnimal")]
        public int IdAnimal { get; set; }
        [Column("nombre")]
        [Required (ErrorMessage =  "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage =  "El nombre no debe exceder 100 caracteres")]
        public required string Nombre { get; set; }
        [Column("codigo_identificacion")]
        [Required(ErrorMessage = "El codigo no es obligatorio")]
        [MaxLength(100, ErrorMessage =  "El codigo no debe exceder 100 caracteres")]
        public required string CodigoIdentificacion { get; set; }
        [Column("fecha_ingreso")]
        [Required(ErrorMessage = "La fecha ingreso es obligatorio")]
        public DateTime FechaIngreso { get; set; }
        [Column("castrado")]
        public bool Castrado { get; set; }

        // Foreign Keys
        [Column("IdEspecie")]
        [Required(ErrorMessage = "La Especie es obligatorio")]
        public int IdEspecie { get; set; }
        [Column("IdRaza")]
        public int? IdRaza { get; set; }
        [Column("IdSexo")]
        public int? IdSexo { get; set; }
        [Column("IdTipo")]
        public int? IdTipo { get; set; }
        [Column("IdEstado")]
        public int? IdEstado { get; set; }
    }
}
