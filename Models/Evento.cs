using System;
using System.Collections.Generic;
using BackendScout.Models; // 👈 Asegúrate de que este sea el namespace donde está Unidad

namespace BackendScout.Models
{
    public class Evento
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string? ImagenUrl { get; set; }

        public string Nivel { get; set; } = "Unidad"; // Unidad / Grupo / Distrito / Nacional
        public Guid? OrganizadorUnidadId { get; set; } // Puede ser null si el evento no es de unidad
        public Unidad OrganizadorUnidad { get; set; } // ✅ Navegación

        public int? OrganizadorGrupoId { get; set; }
        public int? OrganizadorDistritoId { get; set; }
        public bool EsNacional => Nivel == "Nacional";

        public List<string> RamasDestino { get; set; } = new();
        public int? CupoMaximo { get; set; }

        // Relaciones
        public ICollection<UsuarioEvento> Participantes { get; set; }
        public ICollection<EventoOrganizador> Organizadores { get; set; }
    }
}
