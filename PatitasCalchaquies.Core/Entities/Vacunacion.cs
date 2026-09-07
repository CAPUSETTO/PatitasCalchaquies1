using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class Vacunacion
    {
        public int IdVacunacion { get; set; }
        public int IdAnimal { get; set; }
        public int IdVacuna { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? ProximaFecha { get; set; }
        public string? Observacion { get; set; }
    }
}
