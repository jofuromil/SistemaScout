namespace BackendScout.Models
{
    public class ObjetivoEducativo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Rama { get; set; } = string.Empty; // Lobatos, Exploradores, etc.
        public int EdadMinima { get; set; }
        public int EdadMaxima { get; set; }
        public string Area { get; set; } = string.Empty; // Área de crecimiento
        public string Descripcion { get; set; } = string.Empty; // Texto del objetivo
        public string NivelProgresion { get; set; } = string.Empty;        
    }
}
