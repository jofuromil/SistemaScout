namespace BackendScout.Models
{
    public class FichaMedica
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UsuarioId { get; set; } // Relación con el usuario

        // Comunes
        public string Direccion { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public string TipoSangre { get; set; } = string.Empty;
        public string Alergias { get; set; } = string.Empty;
        public string CondicionesAlimentarias { get; set; } = string.Empty;
        public string Medicamentos { get; set; } = string.Empty;
        public string ObservacionesMedicas { get; set; } = string.Empty;
        public string SeguroSalud { get; set; } = string.Empty;

        // Solo scouts
        public string? Colegio { get; set; }
        public string? Curso { get; set; }
        public string? NombrePadre { get; set; }
        public string? TelefonoPadre { get; set; }
        public string? NombreMadre { get; set; }
        public string? TelefonoMadre { get; set; }

        // Solo dirigentes
        public string? Profesion { get; set; }
        public string? NivelFormacion { get; set; }
        public string? NombreContactoEmergencia { get; set; }
        public string? TelefonoContactoEmergencia { get; set; }
    }
}
