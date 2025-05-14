using System;
using BackendScout.Models;

namespace BackendScout.Models
{
    public class CrearRequisitoRequest
    {
        public TipoRequisito Tipo  { get; set; }
        public string Texto        { get; set; } = null!;
    }
}
