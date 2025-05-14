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

        public async Task<List<Mensaje>> ObtenerMensajesPorUnidad(Guid unidadId)
        {
            return await _context.Mensajes
                .Where(m => m.UnidadId == unidadId)
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();
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
