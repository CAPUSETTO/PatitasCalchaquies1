using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class Esterilizacion
    {
        public int IdEsterilizacion { get; set; }
        public int IdAnimal { get; set; } // foreign key 
        public int IdVeterinario { get; set; } // foreign key
        public DateTime Fecha { get; set; }
        public required string Tipo { get; set; }
        public string? Observacion { get; set; }
    }
}
