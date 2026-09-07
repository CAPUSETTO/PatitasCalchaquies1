using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class Rol
    {
        public int IdRol { get; set; }
        public required string Nombre { get; set; }
        public string? descripcion { get; set; }
    }
}
