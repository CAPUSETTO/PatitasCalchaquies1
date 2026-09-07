using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class Atencion_Veterinario
    {
        public int IdAtencionVeterinaria { get; set; }
        public DateTime Fecha { get; set; }
        public required string motivo { get; set; }

        // foreign key

        public int IdAnimal { get; set; }
        public int IdVeterinario { get; set; }
    }
}
