using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class Sector
    {
        public int IdSector { get; set; }
        public required string Nombre { get; set; }
        public int? Capacidad { get; set; }
        public string? Descripcion { get; set; }

        // Foreign Keys
        public int IdPredio { get; set; }
    }
}
