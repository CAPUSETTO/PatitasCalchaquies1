using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    [Table("Medicamento")]
    public class Medicamento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdMedicamento")]
        public int IdMedicamento { get; set; }
        [Column("Nombre")]
        [MaxLength(100)]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public required string Nombre { get; set; }
        [Column("Descripcion")]
        [MaxLength(100)]
        public string? Descripcion { get; set; }
    }
}
