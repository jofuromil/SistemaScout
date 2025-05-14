public class Evento
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    
    public int TipoEventoId { get; set; }
    public TipoEvento TipoEvento { get; set; }

    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string? ImagenUrl { get; set; }

    public string Nivel { get; set; } = "Unidad"; // Unidad / Grupo / Distrito / Nacional
    public int? OrganizadorUnidadId { get; set; } // Puede ser null si el evento no es de unidad
    public int? OrganizadorGrupoId { get; set; }
    public int? OrganizadorDistritoId { get; set; }
    public bool EsNacional => Nivel == "Nacional";

    public List<string> RamasDestino { get; set; } = new();
    public int? CupoMaximo { get; set; }

    // Relaciones
    public ICollection<UsuarioEvento> Participantes { get; set; }
    public ICollection<EventoOrganizador> Organizadores { get; set; }
}
