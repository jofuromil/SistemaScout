using BackendScout.DTOs;
using BackendScout.Services;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using BackendScout.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BackendScout.Models;

namespace BackendScout.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly EventoService _eventoService;
        private readonly DocumentoEventoService _documentoService;
        private readonly AppDbContext _context;
        public EventoController(EventoService eventoService, AppDbContext context)
{
    _eventoService = eventoService;
    _context = context;
}
        // ...aquí siguen tus métodos...

        [HttpPost("unidad")]
public async Task<IActionResult> CrearEventoPorUnidad([FromBody] CrearEventoUnidadRequest request)
{
    try
    {
        var creado = await _eventoService.CrearEventoPorCodigoUnidadAsync(request);
        return Ok(new { mensaje = "Evento creado correctamente" });
    }
    catch (Exception ex)
    {
        return BadRequest(new
        {
            mensaje = ex.Message,
            detalle = ex.InnerException?.Message
        });
    }
}

        [HttpPost]
    public async Task<IActionResult> CrearEvento([FromBody] Evento evento)
    {
        var creado = await _eventoService.CrearEventoAsync(evento);
        return Ok(creado);
    }
    [HttpGet("unidad/{codigoUnidad}")]
public async Task<IActionResult> EventosPorUnidad(string codigoUnidad)
{
    var unidad = await _eventoService.ObtenerUnidadPorCodigoAsync(codigoUnidad);
    if (unidad == null)
        return NotFound(new { mensaje = "Unidad no encontrada con el código proporcionado." });

    var eventos = await _eventoService.ObtenerEventosDeUnidadAsync(unidad.Id);
    return Ok(eventos);
}


    [HttpGet]
    public async Task<IActionResult> ListarEventos()
    {
        var eventos = await _eventoService.ListarEventosAsync();
        return Ok(eventos);
    }
    [HttpPost("inscribirse")]
public async Task<IActionResult> InscribirseEvento([FromBody] InscripcionEventoRequest request)
{
    try
    {
        var mensaje = await _eventoService.InscribirUsuarioAsync(
            request.EventoId,
            request.UsuarioId,
            request.TipoParticipacion
        );

        return Ok(new { mensaje });
    }
    catch (Exception ex)
    {
        return BadRequest(new { mensaje = ex.Message });
    }
}
[HttpGet("mis-eventos/{usuarioId}")]
public async Task<IActionResult> MisEventos(Guid usuarioId)
{
    try
    {
        var lista = await _eventoService.ObtenerEventosDeUsuarioAsync(usuarioId);

        var resultado = lista.Select(u => new
        {
            u.Evento.Id,
            u.Evento.Nombre,
            u.Evento.FechaInicio,
            u.Evento.FechaFin,
            u.Estado,
            u.TipoParticipacion
        });

        return Ok(resultado);
    }
    catch (Exception ex)
    {
        return BadRequest(new { mensaje = ex.Message });
    }
}
[HttpGet("{eventoId}/inscritos/{dirigenteId}")]
public async Task<IActionResult> VerInscritos(int eventoId, Guid dirigenteId)
{
    try
    {
        var evento = await _context.Eventos.FindAsync(eventoId);
        if (evento == null)
            return NotFound(new { mensaje = "Evento no encontrado." });

        var esOrganizador = evento.OrganizadorUnidadId == 
                        _context.Users
                        .Where(u => u.Id == dirigenteId && u.Tipo == "Dirigente")
                        .Select(u => u.UnidadId)
                        .FirstOrDefault()
    || await _context.OrganizadoresEvento.AnyAsync(o => o.EventoId == eventoId && o.UserId == dirigenteId);

        if (!esOrganizador)
            return StatusCode(403, new { mensaje = "No tienes permiso para ver los inscritos." });

        var lista = await _context.UsuarioEvento
            .Where(u => u.EventoId == eventoId)
            .Include(u => u.User)
            .ToListAsync();

        var resultado = lista.Select(u => new
        {
            u.User.Id,
            u.User.NombreCompleto,
            u.User.Correo,
            u.Estado,
            u.TipoParticipacion
        });

        return Ok(resultado);
    }
    catch (Exception ex)
    {
        return BadRequest(new { mensaje = ex.Message });
    }
}


        [HttpPost("actualizar-estado")]
        [Authorize(Roles = "Dirigente")]
public async Task<IActionResult> ActualizarEstadoInscripcion([FromBody] ActualizarEstadoInscripcionRequest request)
{
    try
    {
        var mensaje = await _eventoService.ActualizarEstadoInscripcionAsync(
            request.EventoId,
            request.UsuarioId,
            request.DirigenteId,
            request.NuevoEstado
        );

        return Ok(new { mensaje });
    }
    catch (UnauthorizedAccessException ex)
    {
        return StatusCode(403, new { mensaje = ex.Message });
    }
    catch (Exception ex)
    {
        return BadRequest(new { mensaje = ex.Message });
    }
}
[HttpPost("agregar-organizador")]
public async Task<IActionResult> AgregarOrganizador([FromBody] AgregarOrganizadorRequest request)
{
    try
    {
        var mensaje = await _eventoService.AgregarOrganizadorAsync(
            request.EventoId,
            request.DirigentePrincipalId,
            request.NuevoOrganizadorId
        );

        return Ok(new { mensaje });
    }
    catch (UnauthorizedAccessException ex)
    {
        return StatusCode(403, new { mensaje = ex.Message });
    }
    catch (Exception ex)
    {
        return BadRequest(new { mensaje = ex.Message });
    }
}
        [HttpGet("listar/{eventoId}")]
        public async Task<IActionResult> ListarArchivosEvento(int eventoId)
        {
            var archivos = await _documentoService.ListarArchivosDeEventoAsync(eventoId);

            var resultado = archivos.Select(a => new
            {
                a.Id,
                a.NombreArchivo,
                a.TipoMime,
                a.FechaSubida,
                url = a.RutaArchivo  // Ruta relativa desde el navegador
            });

            return Ok(resultado);
        }
        [HttpGet("detalle/{id}")]
        public async Task<IActionResult> ObtenerDetalleEvento(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
                return NotFound(new { mensaje = "Evento no encontrado" });

            return Ok(evento);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerEventoPorId(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);

            if (evento == null)
                return NotFound(new { mensaje = "Evento no encontrado." });

            return Ok(evento);
        }

        [HttpPost("enviar-mensaje")]
public async Task<IActionResult> EnviarMensajeEvento([FromBody] EnviarMensajeEventoRequest request)
{
    try
    {
        var mensaje = await _eventoService.EnviarMensajeEventoAsync(
            request.EventoId,
            request.DirigenteId,
            request.Contenido
        );

        return Ok(new { mensaje });
    }
    catch (UnauthorizedAccessException ex)
    {
        return StatusCode(403, new { mensaje = ex.Message });
    }
    catch (Exception ex)
    {
        return BadRequest(new { mensaje = ex.Message });
    }
}
[HttpGet("mensajes-usuario/{usuarioId}")]
public async Task<IActionResult> MensajesDeEventos(Guid usuarioId)
{
    try
    {
        var mensajes = await _eventoService.ObtenerMensajesParaUsuarioAsync(usuarioId);

        var resultado = mensajes.Select(m => new
        {
            m.Id,
            m.Contenido,
            m.FechaEnvio,
            Evento = m.Evento.Nombre,
            Remitente = m.Remitente.NombreCompleto
        });

        return Ok(resultado);
    }
    catch (Exception ex)
    {
        return BadRequest(new { mensaje = ex.Message });
    }
}
[HttpGet("estadisticas/{eventoId}/{dirigenteId}")]
public async Task<IActionResult> EstadisticasEvento(int eventoId, Guid dirigenteId)
{
    try
    {
        var datos = await _eventoService.ObtenerEstadisticasEventoAsync(eventoId, dirigenteId);
        return Ok(datos);
    }
    catch (UnauthorizedAccessException ex)
    {
        return StatusCode(403, new { mensaje = ex.Message });
    }
    catch (Exception ex)
    {
        return BadRequest(new { mensaje = ex.Message });
    }
}
[HttpGet("estadisticas-pdf/{eventoId}/{dirigenteId}")]
public async Task<IActionResult> DescargarEstadisticasPdf(int eventoId, Guid dirigenteId)
{
    try
    {
        var pdfBytes = await _eventoService.GenerarEstadisticasPdfAsync(eventoId, dirigenteId);
        return File(pdfBytes, "application/pdf", $"estadisticas_evento_{eventoId}.pdf");
    }
    catch (UnauthorizedAccessException ex)
    {
        return StatusCode(403, new { mensaje = ex.Message });
    }
    catch (Exception ex)
    {
        return BadRequest(new { mensaje = ex.Message });
    }
}
[HttpGet("exportar-inscritos-pdf/{eventoId}/{dirigenteId}")]
public async Task<IActionResult> ExportarInscritosPdf(int eventoId, Guid dirigenteId)
{
    try
    {
        var inscritos = await _eventoService.ObtenerInscritosEventoAsync(eventoId, dirigenteId);

        var pdf = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4.Landscape());

                page.Header().Text($"Lista de Inscritos al Evento").FontSize(18).Bold().AlignCenter();

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(c =>
                    {
                        c.RelativeColumn(); // Nombre
                        c.RelativeColumn(); // Correo
                        c.RelativeColumn(); // Teléfono
                        c.RelativeColumn(); // Rama
                        c.RelativeColumn(); // Estado
                        c.RelativeColumn(); // Unidad
                        c.RelativeColumn(); // Grupo
                        c.RelativeColumn(); // Distrito
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Nombre").Bold();
                        header.Cell().Text("Correo").Bold();
                        header.Cell().Text("Teléfono").Bold();
                        header.Cell().Text("Rama").Bold();
                        header.Cell().Text("Estado").Bold();
                        header.Cell().Text("Unidad").Bold();
                        header.Cell().Text("Grupo").Bold();
                        header.Cell().Text("Distrito").Bold();
                    });

                    foreach (var i in inscritos)
                    {
                        table.Cell().Text(i.Nombre);
                        table.Cell().Text(i.Correo);
                        table.Cell().Text(i.Telefono);
                        table.Cell().Text(i.Rama);
                        table.Cell().Text(i.Estado);
                        table.Cell().Text(i.Unidad);
                        table.Cell().Text(i.Grupo);
                        table.Cell().Text(i.Distrito);
                    }
                });

                page.Footer().AlignCenter().Text("Generado por Sistema Scout").FontSize(10);
            });
        });

        using var stream = new MemoryStream();
        pdf.GeneratePdf(stream);

        return File(stream.ToArray(), "application/pdf", $"inscritos_evento_{eventoId}.pdf");
    }
    catch (UnauthorizedAccessException ex)
    {
        return StatusCode(403, new { mensaje = ex.Message });
    }
    catch (Exception ex)
    {
        return BadRequest(new { mensaje = ex.Message });
    }
}
[HttpPost("exportar-inscritos-agrupado")]
public async Task<IActionResult> ExportarInscritosAgrupadoPdf([FromBody] ExportarInscritosAgrupadoRequest request)
{
    try
    {
        var inscritos = await _eventoService.ObtenerInscritosEventoAsync(request.EventoId, request.DirigenteId);

        Func<InscritoEventoDto, string> agrupador = request.AgruparPor.ToLower() switch
        {
            "grupo" => i => i.Grupo,
            "unidad" => i => i.Unidad,
            "rama" => i => i.Rama,
            _ => i => i.Distrito // por defecto
        };

        var inscritosAgrupados = inscritos
            .GroupBy(agrupador)
            .OrderBy(g => g.Key)
            .ToList();

        var pdf = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4.Landscape());

                page.Header().Text($"Inscritos agrupados por {request.AgruparPor}")
                             .FontSize(18).Bold().AlignCenter();

                page.Content().Column(col =>
                {
                    foreach (var grupo in inscritosAgrupados)
                    {
                        col.Item().Element(e => e.PaddingBottom(5)).Text($"{request.AgruparPor}: {grupo.Key}")
                                .FontSize(14).Bold().Underline();


                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn(); // Nombre
                                c.RelativeColumn(); // Correo
                                c.RelativeColumn(); // Teléfono
                                c.RelativeColumn(); // Rama
                                c.RelativeColumn(); // Tipo
                                c.RelativeColumn(); // Estado
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Nombre").Bold();
                                header.Cell().Text("Correo").Bold();
                                header.Cell().Text("Teléfono").Bold();
                                header.Cell().Text("Rama").Bold();
                                header.Cell().Text("Tipo").Bold();
                                header.Cell().Text("Estado").Bold();
                            });

                            foreach (var i in grupo)
                            {
                                table.Cell().Text(i.Nombre);
                                table.Cell().Text(i.Correo);
                                table.Cell().Text(i.Telefono);
                                table.Cell().Text(i.Rama);
                                table.Cell().Text(i.TipoUsuario);
                                table.Cell().Text(i.Estado);
                            }
                        });

                        col.Item().PaddingVertical(10);
                    }
                });

                page.Footer().AlignCenter().Text("Generado por Sistema Scout").FontSize(10);
            });
        });

        using var stream = new MemoryStream();
        pdf.GeneratePdf(stream);

        return File(stream.ToArray(), "application/pdf", $"inscritos_agrupados_{request.AgruparPor.ToLower()}.pdf");
    }
    catch (UnauthorizedAccessException ex)
    {
        return StatusCode(403, new { mensaje = ex.Message });
    }
    catch (Exception ex)
    {
        return BadRequest(new { mensaje = ex.Message });
    }
}
[HttpPost("exportar-inscritos-jerarquico")]
public async Task<IActionResult> ExportarListaJerarquica([FromBody] ExportarListaJerarquicaRequest request)

{
    try
    {
        var inscritos = await _eventoService.ObtenerInscritosEventoAsync(request.EventoId, request.DirigenteId);

        // Filtro por estado
        if (!string.IsNullOrWhiteSpace(request.EstadoFiltro))
        {
            inscritos = inscritos
                .Where(i => i.Estado.Equals(request.EstadoFiltro, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // Filtro por rama
        if (!string.IsNullOrWhiteSpace(request.RamaFiltro))
        {
            inscritos = inscritos
                .Where(i => i.Rama.Equals(request.RamaFiltro, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // Filtro por tipo de usuario
        if (!string.IsNullOrWhiteSpace(request.TipoUsuarioFiltro))
        {
            inscritos = inscritos
                .Where(i => i.TipoUsuario.Equals(request.TipoUsuarioFiltro, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }


        var inscritosAgrupados = inscritos
            .GroupBy(i => i.Distrito)
            .OrderBy(g => g.Key)
            .ToList();

        var pdf = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4.Landscape());

                page.Header().Text("Lista jerárquica de inscritos")
                             .FontSize(18).Bold().AlignCenter();

                page.Content().Column(col =>
                {
                    foreach (var distrito in inscritosAgrupados)
                    {
                        col.Item().Element(e => e.PaddingBottom(5)).Text($"Distrito: {distrito.Key}")
                              .FontSize(14).Bold().Underline();


                        var grupos = distrito.GroupBy(i => i.Grupo).OrderBy(g => g.Key);

                        foreach (var grupo in grupos)
                        {
                            col.Item().Element(e => e.PaddingBottom(3)).Text($"  Grupo: {grupo.Key}")
          .FontSize(13).Bold().Italic();


                            var ramas = grupo.GroupBy(i => i.Rama).OrderBy(r => r.Key);

                            foreach (var rama in ramas)
                            {
                                col.Item().Element(e => e.PaddingBottom(2)).Text($"    Rama: {rama.Key}")
          .FontSize(12).Bold();


                                var unidades = rama.GroupBy(i => i.Unidad).OrderBy(u => u.Key);

                                foreach (var unidad in unidades)
                                {
                                    col.Item().Element(e => e.PaddingBottom(2)).Text($"      Unidad: {unidad.Key}")
          .FontSize(11).Bold();


                                    col.Item().Table(table =>
                                    {
                                        table.ColumnsDefinition(c =>
                                        {
                                            c.RelativeColumn(); // Nombre
                                            c.RelativeColumn(); // Correo
                                            c.RelativeColumn(); // Teléfono
                                            c.RelativeColumn(); // Tipo
                                            c.RelativeColumn(); // Estado
                                        });

                                        table.Header(header =>
                                        {
                                            header.Cell().Text("Nombre").Bold();
                                            header.Cell().Text("Correo").Bold();
                                            header.Cell().Text("Teléfono").Bold();
                                            header.Cell().Text("Tipo").Bold();
                                            header.Cell().Text("Estado").Bold();
                                        });

                                        foreach (var i in unidad.OrderBy(i => i.Nombre))
                                        {
                                            table.Cell().Text(i.Nombre);
                                            table.Cell().Text(i.Correo);
                                            table.Cell().Text(i.Telefono);
                                            table.Cell().Text(i.TipoUsuario);
                                            table.Cell().Text(i.Estado);
                                        }
                                    });

                                    col.Item().PaddingVertical(5);
                                }
                            }
                        }

                        col.Item().PaddingVertical(10); // Separación entre distritos
                    }
                });

                page.Footer().AlignCenter().Text("Generado por Sistema Scout").FontSize(10);
            });
        });

        using var stream = new MemoryStream();
        pdf.GeneratePdf(stream);

        return File(stream.ToArray(), "application/pdf", $"lista_jerarquica_evento_{request.EventoId}.pdf");
    }
    catch (UnauthorizedAccessException ex)
    {
        return StatusCode(403, new { mensaje = ex.Message });
    }
    catch (Exception ex)
    {
        return BadRequest(new { mensaje = ex.Message });
    }
}

}
}
