using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class Foto_Animal
    {
        public int IdFotoAnimal { get; set; }
        public required string Ruta { get; set; }
        public bool EsPrincipal { get; set; }
        public DateTime Fecha { get; set; }

        // foreign key

        public int IdAnimal { get; set; }
    }
}
