using BackendScout.Models;

public class UsuarioEvento
{
    public int Id { get; set; }

    public int EventoId { get; set; }
    public Evento Evento { get; set; }

    public Guid UsuarioId { get; set; }
    public User User { get; set; }

    public string Estado { get; set; } = "Pendiente"; // Aceptado / Rechazado / Pendiente
    public string TipoParticipacion { get; set; } = "Participante"; // Solo para dirigentes
}
