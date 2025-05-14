namespace BackendScout.Models
{
    public class Unidad
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = string.Empty;
        public string Rama { get; set; } = string.Empty;
        public string GrupoScout { get; set; } = string.Empty;
        public string Distrito { get; set; } = string.Empty;
        public string CodigoUnidad { get; set; } = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        public Guid DirigenteId { get; set; }  // Quién creó la unidad

    }
}
