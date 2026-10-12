using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class Raza
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdRaza")]
        public int IdRaza { get; set; }
        [Column("Nombre")]
        [Required(ErrorMessage = "La especie es obligatoria")]
        public int IdEspecie { get; set; } // Foreign Key
        [Column("nombre")]
        [MaxLength(50)]
        [Required(ErrorMessage = "El nombre de la raza es obligatorio")]
        public required string Nombre { get; set; }
    }
}
