namespace BackendScout.DTOs
{
    public class AgregarEnlaceDocumentoRequest
    {
        public int EventoId { get; set; }
        public Guid DirigenteId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string TipoMime { get; set; } = "enlace/externo";
    }
}
