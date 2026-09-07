using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class Adopcion
    {
        public int IdAdopcion { get; set; }
        public int IdSolicitudAdopcion { get; set; } // foreign key
        public DateTime Fecha { get; set; }
        public string? Observacion { get; set; }
    }
}
