using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class DevolucionAdopcion
    {
        public int IdDevolucionAdopcion { get; set; }
        public int IdAdopcion { get; set; } // foreign key
        public DateTime Fecha { get; set; }
        public required string Motivo { get; set; }
        public string? Observacion { get; set; }
    }
}
