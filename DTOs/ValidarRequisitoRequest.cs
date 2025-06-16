using System;

namespace BackendScout.Models
{
    public class ValidarRequisitoRequest
    {
        public Guid CumplidoId { get; set; }
        public bool Aprobado { get; set; }
    }
}
