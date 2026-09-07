using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class AlojamientoAnimal
    {
        public int IdAlojamiento { get; set; }
        public int IdAnimal { get; set; } // foreign key
        public int IdSector { get; set; } // foreign key
        public DateTime FechaIngreso { get; set; }
        public DateTime? FechaSalida { get; set; }
        public string? Observacion { get; set; }
    }
}
