using BackendScout.Data;
using BackendScout.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

public class DocumentoEventoService
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public DocumentoEventoService(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<string> SubirArchivoAsync(int eventoId, Guid dirigenteId, IFormFile archivo)
    {
        var esOrganizador = await _context.OrganizadoresEvento
            .AnyAsync(o => o.EventoId == eventoId && o.UserId == dirigenteId);

        if (!esOrganizador)
            throw new UnauthorizedAccessException("No tienes permiso para subir archivos a este evento.");

        var carpeta = Path.Combine(_env.WebRootPath, "archivos", "eventos", eventoId.ToString());

        if (!Directory.Exists(carpeta))
            Directory.CreateDirectory(carpeta);

        var rutaFisica = Path.Combine(carpeta, archivo.FileName);

        using (var stream = new FileStream(rutaFisica, FileMode.Create))
        {
            await archivo.CopyToAsync(stream);
        }

        var rutaRelativa = Path.Combine("archivos", "eventos", eventoId.ToString(), archivo.FileName).Replace("\\", "/");

        var doc = new DocumentoEvento
        {
            EventoId = eventoId,
            NombreArchivo = archivo.FileName,
            RutaArchivo = "/" + rutaRelativa,
            TipoMime = archivo.ContentType,
            SubidoPorId = dirigenteId
        };

        _context.DocumentosEvento.Add(doc);
        await _context.SaveChangesAsync();

        return "Archivo subido correctamente.";
    }
    public async Task<List<DocumentoEvento>> ListarArchivosDeEventoAsync(int eventoId)
    {
    return await _context.DocumentosEvento
        .Where(d => d.EventoId == eventoId)
        .OrderByDescending(d => d.FechaSubida)
        .ToListAsync();
    }
    public async Task<string> AgregarEnlaceExternoAsync(int eventoId, Guid dirigenteId, string nombre, string url, string tipoMime)
{
    var esOrganizador = await _context.OrganizadoresEvento
        .AnyAsync(o => o.EventoId == eventoId && o.UserId == dirigenteId);

    if (!esOrganizador)
        throw new UnauthorizedAccessException("No tienes permiso para agregar documentos al evento.");

    var doc = new DocumentoEvento
    {
        EventoId = eventoId,
        NombreArchivo = nombre,
        RutaArchivo = url,
        TipoMime = tipoMime,
        EsEnlaceExterno = true,
        SubidoPorId = dirigenteId
    };

    _context.DocumentosEvento.Add(doc);
    await _context.SaveChangesAsync();

    return "Enlace externo agregado correctamente.";
}


}
