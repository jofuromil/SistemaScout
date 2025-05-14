using System;
using System.Collections.Generic;

namespace BackendScout.Models
{
    public class Especialidad
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = null!;
        public string Rama { get; set; } = null!;            // Lobatos, Exploradores, Pioneros, Rovers
        public string? Descripcion { get; set; }             // Sólo para Lobatos/Exploradores
        public ICollection<Requisito> Requisitos { get; set; } = new List<Requisito>();
    }
}
