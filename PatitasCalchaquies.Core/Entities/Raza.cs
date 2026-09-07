using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class Raza
    {
        public int IdRaza { get; set; }
        public int? IdEspecie { get; set; } // Foreign Key
        public required string Nombre { get; set; }
    }
}
