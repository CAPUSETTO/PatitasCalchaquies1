using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class Persona
    {
        public int IdPersona { get; set; }

        public required string Dni { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Telefono { get; set; }
        public string? Email { get; set; }
        public required string Direccion { get; set; }

        public DateTime FechaAlta { get; set; }
        public bool Activo { get; set; }

    }
}
