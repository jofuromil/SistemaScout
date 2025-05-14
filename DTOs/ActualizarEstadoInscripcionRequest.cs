namespace BackendScout.DTOs
{
    public class ActualizarEstadoInscripcionRequest
    {
        public int EventoId { get; set; }
        public Guid UsuarioId { get; set; }  // Usuario al que se le cambiará el estado
        public Guid DirigenteId { get; set; } // El dirigente que realiza la acción
        public string NuevoEstado { get; set; } = string.Empty; // "Aceptado" o "Rechazado"
    }
}
