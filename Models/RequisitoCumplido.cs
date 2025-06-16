using System;

namespace BackendScout.Models
{
    public class RequisitoCumplido
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid RequisitoId { get; set; }
        public Requisito Requisito { get; set; } = null!;

        public Guid ScoutId { get; set; } // ID del usuario Scout
        public User Scout { get; set; } = null!;

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public bool AprobadoPorDirigente { get; set; } = false;
        public DateTime? FechaAprobacion { get; set; }
    }
}
