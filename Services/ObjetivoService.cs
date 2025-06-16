using BackendScout.Data;
using BackendScout.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendScout.Services
{
    public class ObjetivoService
    {
        private readonly AppDbContext _context;

        public ObjetivoService(AppDbContext context)
        {
            _context = context;
        }

        // Nuevo método principal de filtrado por Rama y Nivel de Progresión
        public async Task<List<ObjetivoEducativo>> ObtenerPorRamaYNivel(string rama, string? nivelProgresion)
        {
            var query = _context.ObjetivosEducativos
                .Where(o => o.Rama.ToLower() == rama.ToLower());

            if (!string.IsNullOrEmpty(nivelProgresion))
            {
                query = query.Where(o => o.NivelProgresion.ToLower() == nivelProgresion.ToLower());
            }

            return await query.ToListAsync();
        }

        public async Task<ObjetivoSeleccionado> SeleccionarObjetivo(Guid usuarioId, Guid objetivoId)
        {
            var yaSeleccionado = await _context.ObjetivosSeleccionados
                .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId && x.ObjetivoEducativoId == objetivoId);

            if (yaSeleccionado != null)
                throw new Exception("Este objetivo ya fue seleccionado por el usuario.");

            var nuevo = new ObjetivoSeleccionado
            {
                UsuarioId = usuarioId,
                ObjetivoEducativoId = objetivoId
            };

            await _context.ObjetivosSeleccionados.AddAsync(nuevo);
            await _context.SaveChangesAsync();

            return nuevo;
        }

        public async Task<bool> ValidarObjetivo(Guid dirigenteId, Guid seleccionId)
        {
            var seleccion = await _context.ObjetivosSeleccionados.FirstOrDefaultAsync(s => s.Id == seleccionId);
            if (seleccion == null)
                throw new Exception("Selección no encontrada.");

            var scout = await _context.Users.FirstOrDefaultAsync(u => u.Id == seleccion.UsuarioId);
            var dirigente = await _context.Users.FirstOrDefaultAsync(u => u.Id == dirigenteId);

            if (dirigente == null || dirigente.Tipo.ToLower() != "dirigente")
                throw new Exception("Solo los dirigentes pueden validar objetivos.");

            if (dirigente.UnidadId != scout.UnidadId)
                throw new Exception("El dirigente no pertenece a la misma unidad que el scout.");

            seleccion.Validado = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<object>> HistorialDeObjetivos(Guid usuarioId, bool? soloValidados = null)
        {
            var query = _context.ObjetivosSeleccionados
                .Where(s => s.UsuarioId == usuarioId);

            if (soloValidados.HasValue)
            {
                query = query.Where(s => s.Validado == soloValidados.Value);
            }

            var historial = await query
                .Join(_context.ObjetivosEducativos,
                      seleccion => seleccion.ObjetivoEducativoId,
                      objetivo => objetivo.Id,
                      (seleccion, objetivo) => new
                      {
                          Id = objetivo.Id, // 👈 Se agrega esto
                          objetivo.Area,
                          objetivo.Descripcion,
                          objetivo.NivelProgresion,
                          objetivo.Rama,
                          seleccion.FechaSeleccion,
                          seleccion.Validado
                      })
                .OrderBy(o => o.FechaSeleccion)
                .ToListAsync();

            return historial.Cast<object>().ToList();
        }

        public async Task<List<User>> ObtenerScoutsConObjetivosPendientes(Guid dirigenteId)
        {
            var dirigente = await _context.Users.FindAsync(dirigenteId);
            if (dirigente == null || dirigente.Tipo.ToLower() != "dirigente")
                throw new Exception("Solo un dirigente puede ver objetivos pendientes.");

            if (dirigente.UnidadId == null)
                throw new Exception("Este dirigente no está asociado a ninguna unidad.");

            var scouts = await _context.ObjetivosSeleccionados
                .Where(o => o.Validado == false && o.Usuario.UnidadId == dirigente.UnidadId)
                .Select(o => o.Usuario)
                .Distinct()
                .ToListAsync();

            return scouts;
        }

        public async Task<List<User>> ObtenerUsuariosConPendientes()
        {
            return await _context.ObjetivosSeleccionados
                .Where(o => !o.Validado)
                .Select(o => o.Usuario)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<ObjetivoSeleccionado>> ObjetivosPendientesPorUsuario(Guid usuarioId)
        {
            return await _context.ObjetivosSeleccionados
                .Where(os => os.UsuarioId == usuarioId && !os.Validado)
                .Include(os => os.ObjetivoEducativo)
                .ToListAsync();
        }

        public async Task<List<object>> ObtenerPendientesPorDirigente(Guid dirigenteId)
        {
            var dirigente = await _context.Users.FirstOrDefaultAsync(u => u.Id == dirigenteId);

            if (dirigente == null || dirigente.Tipo.ToLower() != "dirigente")
                throw new Exception("El usuario no es un dirigente válido.");

            if (dirigente.UnidadId == null)
                throw new Exception("Este dirigente no está asignado a ninguna unidad.");

            var pendientes = await _context.ObjetivosSeleccionados
                .Where(s => !s.Validado && s.Usuario.UnidadId == dirigente.UnidadId)
                .Include(s => s.Usuario)
                .Include(s => s.ObjetivoEducativo)
                .Select(s => new
                {
                    Scout = s.Usuario.NombreCompleto,
                    Rama = s.Usuario.Rama,
                    Objetivo = s.ObjetivoEducativo.Descripcion,
                    FechaSeleccion = s.FechaSeleccion,
                    IdSeleccion = s.Id
                })
                .ToListAsync();

            return pendientes.Cast<object>().ToList();
        }

        public async Task<List<ObjetivoSeleccionado>> ObtenerPendientesPorScout(Guid usuarioId)
        {
            return await _context.ObjetivosSeleccionados
                .Include(o => o.ObjetivoEducativo)
                .Where(o => o.UsuarioId == usuarioId && !o.Validado)
                .ToListAsync();
        }
        public async Task<List<object>> ObtenerResumenPorScout(Guid usuarioId)
        {
    var objetivos = await _context.ObjetivosSeleccionados
        .Where(o => o.UsuarioId == usuarioId)
        .Include(o => o.ObjetivoEducativo)
        .ToListAsync();

    var resumen = objetivos
        .Where(o => o.ObjetivoEducativo != null)
        .GroupBy(o => new
        {
            Area = o.ObjetivoEducativo.Area,
            Nivel = o.ObjetivoEducativo.NivelProgresion
        })
        .Select(g => new
        {
            areaCrecimiento = g.Key.Area,
            nivelProgresion = g.Key.Nivel,
            total = g.Count(),
            validados = g.Count(o => o.Validado),
            pendientes = g.Count(o => !o.Validado)
        })
        .OrderBy(r => r.nivelProgresion)
        .ThenBy(r => r.areaCrecimiento)
        .ToList();

    return resumen.Cast<object>().ToList();
    }

    }
}
