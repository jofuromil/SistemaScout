using System;

namespace BackendScout.Models
{
    public class DocumentoEvento
    {
        public int Id { get; set; }

        public int EventoId { get; set; }
        public Evento Evento { get; set; }

        public string NombreArchivo { get; set; } = string.Empty;
        public string RutaArchivo { get; set; } = string.Empty;
        public string TipoMime { get; set; } = string.Empty;

        public DateTime FechaSubida { get; set; } = DateTime.Now;

        public Guid SubidoPorId { get; set; }
        public User SubidoPor { get; set; }
        public bool EsEnlaceExterno { get; set; } = false;

    }
}
