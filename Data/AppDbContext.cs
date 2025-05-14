using BackendScout.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendScout.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Unidad> Unidades { get; set; }
        public DbSet<FichaMedica> FichasMedicas { get; set; }
        public DbSet<ObjetivoEducativo> ObjetivosEducativos { get; set; }
        public DbSet<ObjetivoSeleccionado> ObjetivosSeleccionados { get; set; }
        public DbSet<Mensaje> Mensajes { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; } = null!;
        public DbSet<Requisito>   Requisitos   { get; set; } = null!;
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<TipoEvento> TiposEvento { get; set; }
        public DbSet<UsuarioEvento> UsuariosEvento { get; set; }
        public DbSet<EventoOrganizador> OrganizadoresEvento { get; set; }
        public DbSet<DocumentoEvento> DocumentosEvento { get; set; }
        public DbSet<MensajeEvento> MensajesEvento { get; set; }
        public DbSet<MensajeEventoDestinatario> MensajesEventoDestinatarios { get; set; }


    }
}
