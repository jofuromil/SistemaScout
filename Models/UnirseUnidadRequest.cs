namespace BackendScout.Models
{
    public class UnirseUnidadRequest
    {
        public Guid UsuarioId { get; set; }
        public string CodigoUnidad { get; set; } = string.Empty;
    }
}
