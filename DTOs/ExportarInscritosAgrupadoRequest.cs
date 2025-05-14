namespace BackendScout.DTOs
{
    public class ExportarInscritosAgrupadoRequest
    {
        public int EventoId { get; set; }
        public Guid DirigenteId { get; set; }
        public string AgruparPor { get; set; } = "Distrito"; // Valores válidos: Distrito, Grupo, Unidad, Rama
    }
}
