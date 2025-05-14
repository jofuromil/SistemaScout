using BackendScout.Models;

public class EventoOrganizador
{
    public int Id { get; set; }

    public int EventoId { get; set; }
    public Evento Evento { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

}
