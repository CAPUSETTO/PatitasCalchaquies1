using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class Vacuna
    {
        public int IdVacuna { get; set; }
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
    }
}
