using System;

namespace BackendScout.Models
{
    public class MensajeEvento
    {
        public int Id { get; set; }

        public int EventoId { get; set; }
        public Evento Evento { get; set; }

        public Guid RemitenteId { get; set; }
        public User Remitente { get; set; }

        public string Contenido { get; set; } = string.Empty;

        public DateTime FechaEnvio { get; set; } = DateTime.Now;

        public ICollection<MensajeEventoDestinatario> Destinatarios { get; set; } = new List<MensajeEventoDestinatario>();
    }
}
