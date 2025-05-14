namespace BackendScout.DTOs
{
    public class ExportarListaJerarquicaRequest
    {
        public int EventoId { get; set; }
        public Guid DirigenteId { get; set; }
        public string? EstadoFiltro { get; set; }  // Aceptado, Pendiente, Rechazado, o null para todos
        public string? RamaFiltro { get; set; }       // null = todas
        public string? TipoUsuarioFiltro { get; set; } // null = todos

    }
}
