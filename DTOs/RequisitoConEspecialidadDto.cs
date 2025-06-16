namespace BackendScout.Dtos
{
    public class RequisitoConEspecialidadDto
    {
        public Guid CumplidoId { get; set; }
        public string Texto { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string EspecialidadNombre { get; set; } = string.Empty;
    }
}
