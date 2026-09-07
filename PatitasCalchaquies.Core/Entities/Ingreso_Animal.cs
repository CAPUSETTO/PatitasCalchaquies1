using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class IngresoAnimal
    {
        public int IdIngresoAnimal { get; set; }
        public int IdAnimal { get; set; } // fOREIGN KEY
        public DateTime Fecha { get; set; }
        public required string Motivo { get; set; }
        public required string TipoIngreso { get; set; }
        public string? LugarOrigen { get; set; }
        public string? Observacion { get; set; }
    }
}
