using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class PlanAlimentacion
    {
        public int IdPlan { get; set; }
        public int IdAnimal { get; set; } // foreign key
        public required string TipoAlimento { get; set; }
        public decimal Cantidad { get; set; }
        public required string Frecuencia { get; set; }
        public string? Observacion { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
    }
}
