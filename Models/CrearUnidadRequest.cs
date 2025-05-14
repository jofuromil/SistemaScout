using System.ComponentModel.DataAnnotations;

namespace BackendScout.Models
{
    public class CrearUnidadRequest
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Rama { get; set; }

        [Required]
        public string GrupoScout { get; set; }

        [Required]
        public string Distrito { get; set; }

        [Required]
        public Guid DirigenteId { get; set; } // quien crea la unidad
    }
}
