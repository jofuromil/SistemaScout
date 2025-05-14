using System;
using System.ComponentModel.DataAnnotations;

namespace BackendScout.Models
{
    public class CrearMensajeRequest
    {
        [Required]
        public string Contenido { get; set; } = string.Empty;

        [Required]
        public Guid UnidadId { get; set; }

        [Required]
        public Guid DirigenteId { get; set; }
    }
}
