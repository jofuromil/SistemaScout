using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendScout.Models
{
    public class Mensaje
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Contenido { get; set; } = string.Empty;

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [Required]
        public Guid UnidadId { get; set; }

        [Required]
        public Guid DirigenteId { get; set; }
        public string? RutaImagen { get; set; }     // Nuevo campo
        public string? RutaArchivo { get; set; }    // Nuevo campo
        public DateTime? ExpiraEl { get; set; }     // Para manejo automático de expiración

        [ForeignKey("UnidadId")]
        public Unidad? Unidad { get; set; }

        [ForeignKey("DirigenteId")]
        public User? Dirigente { get; set; }
    }
}
