using BackendScout.Data;
using BackendScout.Models;
using BackendScout.DTOs;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

public class EventoService
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public EventoService(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }
    public async Task<Evento> CrearEventoPorCodigoUnidadAsync(CrearEventoUnidadRequest request)
{
    var unidad = await _context.Unidades
        .FirstOrDefaultAsync(u => u.CodigoUnidad == request.CodigoUnidad);

    if (unidad == null)
        throw new Exception("Unidad no encontrada con el código proporcionado.");

    var nuevoEvento = new Evento
{
    Nombre = request.Nombre,
    Descripcion = request.Descripcion,
    FechaInicio = request.FechaInicio,
    FechaFin = request.FechaFin,
    OrganizadorUnidadId = unidad.Id,
    Nivel = "Unidad"
};

    _context.Eventos.Add(nuevoEvento);
    await _context.SaveChangesAsync();

    return nuevoEvento;
}

    public async Task<byte[]> GenerarEstadisticasPdfAsync(int eventoId, Guid dirigenteId)
    {
        var estadisticas = await ObtenerEstadisticasEventoAsync(eventoId, dirigenteId);
        var evento = await _context.Eventos.FindAsync(eventoId);

        var pdf = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Header().Text($"Estadísticas del Evento: {evento?.Nombre ?? "Sin nombre"}")
                             .FontSize(20).Bold().AlignCenter();

                page.Content().Column(col =>
                {
                    col.Spacing(15);

                    col.Item().Text($"Total Inscritos: {estadisticas.TotalInscritos}");
                    col.Item().Text($"Aceptados: {estadisticas.Aceptados}");
                    col.Item().Text($"Pendientes: {estadisticas.Pendientes}");
                    col.Item().Text($"Rechazados: {estadisticas.Rechazados}");

                    col.Item().Text("Participación por Tipo:").Bold();
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn();
                            c.ConstantColumn(50);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Tipo").Bold();
                            header.Cell().Text("Cantidad").Bold();
                        });

                        foreach (var tipo in estadisticas.ParticipacionPorTipo)
                        {
                            table.Cell().Text(tipo.Key);
                            table.Cell().Text(tipo.Value.ToString());
                        }
                    });

                    col.Item().Text("Participación por Rama:").Bold();
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn();
                            c.ConstantColumn(50);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Rama").Bold();
                            header.Cell().Text("Cantidad").Bold();
                        });

                        foreach (var rama in estadisticas.ParticipacionPorRama)
                        {
                            table.Cell().Text(rama.Key);
                            table.Cell().Text(rama.Value.ToString());
                        }
                    });
                });

                page.Footer().AlignCenter().Text(txt =>
                {
                    txt.Span("Generado por Sistema Scout").FontSize(10);
                });
            });
        });

        using var stream = new MemoryStream();
        pdf.GeneratePdf(stream);
        return stream.ToArray();
    }

    public async Task<Evento> CrearEventoAsync(Evento nuevoEvento)
    {
        _context.Eventos.Add(nuevoEvento);
        await _context.SaveChangesAsync();
        return nuevoEvento;
    }

    public async Task<List<Evento>> ListarEventosAsync()
{
    return await _context.Eventos.ToListAsync(); // ✅ corregido
}
public async Task<Unidad?> ObtenerUnidadPorCodigoAsync(string codigoUnidad)
{
    return await _context.Unidades.FirstOrDefaultAsync(u => u.CodigoUnidad == codigoUnidad);
}
public async Task<List<Evento>> ObtenerEventosDeUnidadAsync(Guid unidadId)
{
    try
    {
        return await _context.Eventos
            .Where(e => e.OrganizadorUnidadId == unidadId)
            .OrderByDescending(e => e.FechaInicio)
            .Select(e => new Evento
            {
                Id = e.Id,
                Nombre = e.Nombre,
                FechaInicio = e.FechaInicio,
                FechaFin = e.FechaFin,
                Descripcion = e.Descripcion,
                ImagenUrl = e.ImagenUrl,
                Nivel = e.Nivel,
                OrganizadorUnidadId = e.OrganizadorUnidadId,
                OrganizadorGrupoId = e.OrganizadorGrupoId,
                OrganizadorDistritoId = e.OrganizadorDistritoId,
                RamasDestino = e.RamasDestino,
                CupoMaximo = e.CupoMaximo
            })
            .ToListAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"🔥 ERROR al cargar eventos: {ex.Message}");
        return new List<Evento>(); // previene crash total
    }
}

public async Task<string> InscribirUsuarioAsync(int eventoId, Guid usuarioId, string? tipoParticipacion)
{
    var evento = await _context.Eventos.FindAsync(eventoId);
    if (evento == null)
    {
        Console.WriteLine($"❌ Evento con ID {eventoId} no encontrado.");
        throw new Exception("El evento no existe.");
    }

    var usuario = await _context.Users.FindAsync(usuarioId);
    if (usuario == null)
    {
        Console.WriteLine($"❌ Usuario con ID {usuarioId} no encontrado.");
        throw new Exception("El usuario no existe.");
    }

    var yaInscrito = await _context.UsuarioEvento
        .FirstOrDefaultAsync(u => u.EventoId == eventoId && u.UsuarioId == usuarioId);

    if (yaInscrito != null)
        return "Ya estás inscrito en este evento.";

    var nuevo = new UsuarioEvento
    {
        EventoId = eventoId,
        UsuarioId = usuarioId,
        TipoParticipacion = tipoParticipacion ?? "Participante",
        Estado = "Pendiente"
    };

    try
    {
        _context.UsuarioEvento.Add(nuevo);
        await _context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ ERROR al guardar inscripción:");
        Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
        throw;
    }

    return "Inscripción enviada correctamente.";
}



    public async Task<List<UsuarioEvento>> ObtenerEventosDeUsuarioAsync(Guid usuarioId)
    {
        return await _context.UsuarioEvento
            .Where(u => u.UsuarioId == usuarioId)
            .Include(u => u.Evento)
            .ToListAsync();
    }

   public async Task<List<UsuarioEvento>> ObtenerInscritosDelEventoAsync(int eventoId, Guid dirigenteId)
{
    var evento = await _context.Eventos.FindAsync(eventoId);

    if (evento == null)
        throw new Exception("Evento no encontrado");

    var esOrganizador = evento.OrganizadorUnidadId == dirigenteId
        || await _context.OrganizadoresEvento.AnyAsync(o => o.EventoId == eventoId && o.UserId == dirigenteId);

    if (!esOrganizador)
        throw new UnauthorizedAccessException("No tienes permiso para ver los inscritos de este evento.");

    return await _context.UsuarioEvento
        .Where(u => u.EventoId == eventoId)
        .Include(u => u.User)
        .ToListAsync();
}

public async Task<string> ActualizarEstadoInscripcionAsync(int eventoId, Guid usuarioId, Guid dirigenteId, string nuevoEstado)
{
    // Obtener el evento con su unidad organizadora
    var evento = await _context.Eventos
        .Include(e => e.OrganizadorUnidad)
        .FirstOrDefaultAsync(e => e.Id == eventoId);

    if (evento == null)
        throw new InvalidOperationException("El evento no existe.");

    // Verificar si el dirigente es organizador directo o pertenece a la unidad organizadora
    var esOrganizador = await _context.OrganizadoresEvento
        .AnyAsync(o => o.EventoId == eventoId && o.UserId == dirigenteId);

    var perteneceAMismaUnidad = await _context.Users
        .AnyAsync(u => u.Id == dirigenteId && u.UnidadId == evento.OrganizadorUnidadId);

    if (!esOrganizador && !perteneceAMismaUnidad)
        throw new UnauthorizedAccessException("No tienes permiso para modificar inscripciones en este evento.");

    // Buscar inscripción
    var inscripcion = await _context.UsuarioEvento
        .FirstOrDefaultAsync(u => u.EventoId == eventoId && u.UsuarioId == usuarioId);

    if (inscripcion == null)
        throw new InvalidOperationException("La inscripción no existe.");

    inscripcion.Estado = nuevoEstado;
    await _context.SaveChangesAsync();

    return $"Estado actualizado a {nuevoEstado}.";
}

    public async Task<string> AgregarOrganizadorAsync(int eventoId, Guid principalId, Guid nuevoOrganizadorId)
    {
        var esOrganizador = await _context.OrganizadoresEvento
            .AnyAsync(o => o.EventoId == eventoId && o.UserId == principalId);

        if (!esOrganizador)
            throw new UnauthorizedAccessException("No tienes permisos para agregar organizadores a este evento.");

        var yaExiste = await _context.OrganizadoresEvento
            .AnyAsync(o => o.EventoId == eventoId && o.UserId == nuevoOrganizadorId);

        if (yaExiste)
            return "El dirigente ya es organizador del evento.";

        var nuevo = new EventoOrganizador
        {
            EventoId = eventoId,
            UserId = nuevoOrganizadorId
        };

        _context.OrganizadoresEvento.Add(nuevo);
        await _context.SaveChangesAsync();

        return "Organizador agregado correctamente.";
    }

    public async Task<string> EnviarMensajeEventoAsync(int eventoId, Guid dirigenteId, string contenido)
    {
        var esOrganizador = await _context.OrganizadoresEvento
            .AnyAsync(o => o.EventoId == eventoId && o.UserId == dirigenteId);

        if (!esOrganizador)
            throw new UnauthorizedAccessException("No tienes permiso para enviar mensajes en este evento.");

        var mensaje = new MensajeEvento
        {
            EventoId = eventoId,
            RemitenteId = dirigenteId,
            Contenido = contenido
        };

        var destinatarios = await _context.UsuarioEvento
            .Where(u => u.EventoId == eventoId && u.Estado == "Aceptado")
            .Select(u => new MensajeEventoDestinatario
            {
                UsuarioId = u.UsuarioId,
                MensajeEvento = mensaje
            }).ToListAsync();

        mensaje.Destinatarios = destinatarios;

        _context.MensajesEvento.Add(mensaje);
        await _context.SaveChangesAsync();

        return "Mensaje enviado a los inscritos aceptados.";
    }

    public async Task<List<MensajeEvento>> ObtenerMensajesParaUsuarioAsync(Guid usuarioId)
    {
        return await _context.MensajesEventoDestinatarios
            .Where(d => d.UsuarioId == usuarioId)
            .Include(d => d.MensajeEvento)
                .ThenInclude(m => m.Evento)
            .Include(d => d.MensajeEvento)
                .ThenInclude(m => m.Remitente)
            .Select(d => d.MensajeEvento)
            .OrderByDescending(m => m.FechaEnvio)
            .ToListAsync();
    }

    public async Task<EstadisticasEventoResponse> ObtenerEstadisticasEventoAsync(int eventoId, Guid dirigenteId)
    {
        var esOrganizador = await _context.OrganizadoresEvento
            .AnyAsync(o => o.EventoId == eventoId && o.UserId == dirigenteId);

        if (!esOrganizador)
            throw new UnauthorizedAccessException("No tienes acceso a este evento.");

        var inscritos = await _context.UsuarioEvento
            .Where(u => u.EventoId == eventoId)
            .Include(u => u.User)
            .ToListAsync();

        var estadisticas = new EstadisticasEventoResponse
        {
            TotalInscritos = inscritos.Count,
            Aceptados = inscritos.Count(i => i.Estado == "Aceptado"),
            Pendientes = inscritos.Count(i => i.Estado == "Pendiente"),
            Rechazados = inscritos.Count(i => i.Estado == "Rechazado"),
            ParticipacionPorTipo = inscritos
                .Where(i => i.User.Tipo == "Dirigente")
                .GroupBy(i => i.TipoParticipacion ?? "Sin especificar")
                .ToDictionary(g => g.Key, g => g.Count()),
            ParticipacionPorRama = inscritos
                .GroupBy(i => i.User.Rama ?? "Sin rama")
                .ToDictionary(g => g.Key, g => g.Count())
        };

        return estadisticas;
    }
    public async Task<List<InscritoEventoDto>> ObtenerInscritosEventoAsync(int eventoId, Guid dirigenteId)
    {
    var esOrganizador = await _context.OrganizadoresEvento
        .AnyAsync(o => o.EventoId == eventoId && o.UserId == dirigenteId);

    if (!esOrganizador)
        throw new UnauthorizedAccessException("No tienes acceso a los inscritos de este evento.");

    var inscritos = await _context.UsuarioEvento
        .Where(u => u.EventoId == eventoId)
        .Include(u => u.User)
            .ThenInclude(u => u.Unidad)
        .ToListAsync();

    return inscritos.Select(i => new InscritoEventoDto
    {
        Nombre = i.User.NombreCompleto,
        Correo = i.User.Correo,
        Telefono = i.User.Telefono,
        Rama = i.User.Rama ?? "",
        TipoUsuario = i.User.Tipo,
        TipoParticipacion = i.TipoParticipacion ?? "",
        Estado = i.Estado,

        Unidad = i.User.Unidad?.Nombre ?? "Sin unidad",
        Grupo = i.User.Unidad?.GrupoScout ?? "Sin grupo",
        Distrito = i.User.Unidad?.Distrito ?? "Sin distrito"
    }).ToList();
    }

}
