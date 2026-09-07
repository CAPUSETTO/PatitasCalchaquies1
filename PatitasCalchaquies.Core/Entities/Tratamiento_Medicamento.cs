using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class TratamientoMedicamento
    {
        public int IdTratamientoMedico { get; set; }
        public int IdMedicamento { get; set; } // foreign key
        public int IdTratamiento { get; set; } // foreign key
        public required string Dosis { get; set; }
        public required string Frecuencia { get; set; }
        public required string Duracion { get; set; }
        public string? Observacion { get; set; }
    }
}
