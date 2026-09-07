using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class Medicamento
    {
        public int IdMedicamento { get; set; }
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
    }
}
