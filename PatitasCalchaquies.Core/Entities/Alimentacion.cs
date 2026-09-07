using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class Alimentacion
    {
        public int IdAlimentacion { get; set; }
        public int IdAnimal { get; set; } // foreign key
        public DateTime FechaHora { get; set; } 
        public required string TipoAlimento { get; set; }
        public decimal Cantidad { get; set; }
        public string? Observacion { get; set; }
    }
}
