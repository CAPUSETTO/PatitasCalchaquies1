using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class Veterinario
    {
        public int IdVeterinario { get; set; }
        public required string Matricula { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Telefono { get; set; }
    }
}
