using BackendScout.Models;

public class TipoEvento
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;

    // Relación inversa opcional
    public ICollection<Evento> Eventos { get; set; }
}
