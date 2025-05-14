using System;

namespace BackendScout.Models
{
    public class Requisito
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid EspecialidadId { get; set; }
        public Especialidad Especialidad { get; set; } = null!;

        public TipoRequisito Tipo { get; set; }
        public string Texto { get; set; } = null!;
    }
}
