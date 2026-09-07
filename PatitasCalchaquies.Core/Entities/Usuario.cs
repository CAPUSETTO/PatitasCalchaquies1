using System;
using System.Collections.Generic;
using System.Text;

namespace PatitasCalchaquies.Core.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public required string User { get; set; }
        public required string PasswordHash { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? UltimoAcceso { get; set; }

    }
}
