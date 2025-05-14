namespace BackendScout.DTOs
{
    public class EnviarMensajeEventoRequest
    {
        public int EventoId { get; set; }
        public Guid DirigenteId { get; set; }
        public string Contenido { get; set; } = string.Empty;
    }
}
