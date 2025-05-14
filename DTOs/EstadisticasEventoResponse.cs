namespace BackendScout.DTOs
{
    public class EstadisticasEventoResponse
    {
        public int TotalInscritos { get; set; }
        public int Aceptados { get; set; }
        public int Pendientes { get; set; }
        public int Rechazados { get; set; }

        public Dictionary<string, int> ParticipacionPorTipo { get; set; } = new();
        public Dictionary<string, int> ParticipacionPorRama { get; set; } = new();
    }
}
