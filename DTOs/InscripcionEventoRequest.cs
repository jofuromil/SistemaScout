namespace BackendScout.DTOs
{
    public class InscripcionEventoRequest
    {
        public int EventoId { get; set; }
        public Guid UsuarioId { get; set; }
        public string? TipoParticipacion { get; set; } // Solo para dirigentes
    }
}
