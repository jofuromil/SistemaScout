using System;

namespace BackendScout.Models
{
    public class MensajeEventoDestinatario
    {
        public int Id { get; set; }

        public int MensajeEventoId { get; set; }
        public MensajeEvento MensajeEvento { get; set; }

        public Guid UsuarioId { get; set; }
        public User Usuario { get; set; }
    }
}
