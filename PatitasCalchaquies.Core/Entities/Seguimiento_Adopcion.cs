using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class SeguimientoAdopcion
    {
        public int IdSeguimientoAdopcion { get; set; }
        public int IdAdopcion { get; set; } // foreign key
        public DateTime Fecha { get; set; }
        public required string Tipo { get; set; }
        public required string Resultado { get; set; }
        public string? Observacion { get; set; }
    }
}
