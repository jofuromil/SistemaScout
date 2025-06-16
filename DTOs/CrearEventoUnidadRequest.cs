namespace BackendScout.DTOs
{
    public class CrearEventoUnidadRequest
    {
        public string Nombre { get; set; }
        public string Lugar { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string? Observaciones { get; set; }
        public string CodigoUnidad { get; set; }
    }
}
