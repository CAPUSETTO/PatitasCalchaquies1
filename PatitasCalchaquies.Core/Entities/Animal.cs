using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class Animal
    {
        public int IdAnimal { get; set; }
        public required string Nombre { get; set; }
        public required string CodigoIdentificacion { get; set; }
        public DateTime FechaIngreso { get; set; }
        public bool Castrado { get; set; }

        // Foreign Keys
        public int IdEspecie { get; set; }
        public int? IdRaza { get; set; }
        public int? IdSexo { get; set; }
        public int? IdEstado { get; set; }
    }
}
