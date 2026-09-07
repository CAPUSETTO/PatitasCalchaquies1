using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class Tratamiento
    {
        public int IdTratamiento { get; set; }
        public int IdAtencionVeterinaria { get; set; } // foreign key
        public required string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public required string Estado { get; set; }
        public string? Observacion { get; set; }
    }
}
