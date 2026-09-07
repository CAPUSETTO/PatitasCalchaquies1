using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class Predio
    {
        public int IdPredio { get; set; }
        public required string Nombre { get; set; }
        public string? direccion { get; set; }
        public string? descripcion { get; set; }

    }
}
