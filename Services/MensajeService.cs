using BackendScout.Data;
using BackendScout.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendScout.Services
{
    public class MensajeService
    {
        private readonly AppDbContext _context;

        public MensajeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Mensaje> CrearMensaje(Mensaje mensaje)
        {
            mensaje.Fecha = DateTime.UtcNow;
            _context.Mensajes.Add(mensaje);
            await _context.SaveChangesAsync();
            return mensaje;
        }

        public async Task<Guid> ObtenerUnidadIdPorDirigente(Guid dirigenteId)
        {
            var unidad = await _context.Unidades
                .FirstOrDefaultAsync(u => u.DirigenteId == dirigenteId);

            return unidad?.Id ?? Guid.Empty;
        }

        public async Task<List<object>> ObtenerMensajesPorUnidad(Guid unidadId)
        {
            return await _context.Mensajes
                .Include(m => m.Dirigente)
                .Where(m => m.UnidadId == unidadId)
                .OrderByDescending(m => m.Fecha)
                .Select(m => new
                {
                    m.Id,
                    m.Contenido,
                    m.Fecha,
                    m.RutaImagen,
                    m.RutaArchivo,
                    NombreDirigente = m.Dirigente != null ? m.Dirigente.NombreCompleto : "(desconocido)"
                })
                .ToListAsync<object>();
        }
        public async Task<bool> EliminarMensaje(Guid id, Guid dirigenteId)
{
    var mensaje = await _context.Mensajes.FirstOrDefaultAsync(m => m.Id == id && m.DirigenteId == dirigenteId);
    if (mensaje == null) return false;

    // Eliminar archivos del sistema si existen
    if (!string.IsNullOrEmpty(mensaje.RutaImagen))
    {
        var rutaImagen = Path.Combine(Directory.GetCurrentDirectory(), mensaje.RutaImagen.Replace("archivos", "ArchivosMensajes"));
        if (File.Exists(rutaImagen))
            File.Delete(rutaImagen);
    }

    if (!string.IsNullOrEmpty(mensaje.RutaArchivo))
    {
        var rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), mensaje.RutaArchivo.Replace("archivos", "ArchivosMensajes"));
        if (File.Exists(rutaArchivo))
            File.Delete(rutaArchivo);
    }

    _context.Mensajes.Remove(mensaje);
    await _context.SaveChangesAsync();
    return true;
}
        public async Task<List<Mensaje>> ObtenerMensajesPorDirigente(Guid dirigenteId)
        {
            return await _context.Mensajes
                .Where(m => m.DirigenteId == dirigenteId)
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();
        }
    }
}

