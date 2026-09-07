using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class SolicitudAdopcion
    {
        public int IdSolicitudAdopcion { get; set; }
        public int IdPersona { get; set; } // foreign key 
        public int IdAnimal { get; set; } // foreign key
        public DateTime Fecha { get; set; }
        public required string Estado { get; set; }
        public string? Observacion { get; set; }
    }
}
