using BackendScout.Models;
using Microsoft.EntityFrameworkCore;
using BackendScout.Data;
using BackendScout.Dtos;
using BackendScout.Dtos.Especialidades;

public class EspecialidadService
{
    private readonly AppDbContext _context;

    public EspecialidadService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> MarcarRequisitoCumplidoAsync(Guid requisitoId, string scoutId)
    {
        Guid scoutGuid = Guid.Parse(scoutId);
        var yaExiste = await _context.RequisitoCumplidos
            .AnyAsync(rc => rc.RequisitoId == requisitoId && rc.ScoutId == scoutGuid);
        if (yaExiste) return false;

        var nuevo = new RequisitoCumplido
        {
            RequisitoId = requisitoId,
            ScoutId = scoutGuid
        };

        _context.RequisitoCumplidos.Add(nuevo);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ValidarRequisitoAsync(Guid cumplidoId, bool aprobado)
    {
        var cumplimiento = await _context.RequisitoCumplidos.FindAsync(cumplidoId);
        if (cumplimiento == null) return false;

        cumplimiento.AprobadoPorDirigente = aprobado;
        cumplimiento.FechaAprobacion = aprobado ? DateTime.UtcNow : null;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ResumenEspecialidadDto>> ObtenerResumenPorScoutAsync(string scoutId)
    {
        Guid scoutGuid = Guid.Parse(scoutId);

        var resumen = await _context.Especialidades
            .Include(e => e.Requisitos)
            .Select(e => new ResumenEspecialidadDto
            {
                EspecialidadId = e.Id,
                Nombre = e.Nombre,
                TotalRequisitos = e.Requisitos.Count,
                Seleccionados = e.Requisitos.Count(r =>
                    _context.RequisitoCumplidos.Any(rc => rc.RequisitoId == r.Id && rc.ScoutId == scoutGuid)),
                Aprobados = e.Requisitos.Count(r =>
                    _context.RequisitoCumplidos.Any(rc => rc.RequisitoId == r.Id && rc.ScoutId == scoutGuid && rc.AprobadoPorDirigente)),
                Cumplida = e.Requisitos.All(r =>
                    _context.RequisitoCumplidos.Any(rc => rc.RequisitoId == r.Id && rc.ScoutId == scoutGuid && rc.AprobadoPorDirigente))
            })
            .Where(r => r.Seleccionados > 0)
            .ToListAsync();

        return resumen;
    }

    public async Task<object?> ObtenerAvanceEspecialidadAsync(Guid especialidadId, string scoutId)
    {
        Guid scoutGuid = Guid.Parse(scoutId);

        var especialidad = await _context.Especialidades
            .Include(e => e.Requisitos)
            .FirstOrDefaultAsync(e => e.Id == especialidadId);
        if (especialidad == null) return null;

        var requisitos = especialidad.Requisitos;
        var requisitosIds = requisitos.Select(r => r.Id).ToList();

        var cumplidos = await _context.RequisitoCumplidos
            .Where(rc => rc.ScoutId == scoutGuid && requisitosIds.Contains(rc.RequisitoId))
            .ToListAsync();

        var resultado = requisitos.Select(r => new
        {
            RequisitoId = r.Id,
            Texto = r.Texto,
            Tipo = r.Tipo.ToString(),
            Seleccionado = cumplidos.Any(c => c.RequisitoId == r.Id),
            Aprobado = cumplidos.Any(c => c.RequisitoId == r.Id && c.AprobadoPorDirigente)
        });

        return new
        {
            EspecialidadId = especialidad.Id,
            especialidad.Nombre,
            especialidad.Descripcion,
            Requisitos = resultado
        };
    }

    public async Task<List<Especialidad>> ObtenerPorRamaAsync(string rama)
    {
        return await _context.Especialidades
            .Where(e => e.Rama == rama)
            .ToListAsync();
    }

    public async Task<List<ResumenEspecialidadDto>> ObtenerResumenScout(Guid scoutId)
    {
        var cumplidos = await _context.RequisitoCumplidos
            .Where(r => r.ScoutId == scoutId)
            .Include(rc => rc.Requisito)
            .ThenInclude(r => r.Especialidad)
            .ToListAsync();

        var resumen = cumplidos
            .GroupBy(c => c.Requisito.Especialidad.Nombre)
            .Select(g => new ResumenEspecialidadDto
            {
                EspecialidadId = Guid.NewGuid(),
                Nombre = g.Key,
                Seleccionados = g.Count(),
                Aprobados = g.Count(r => r.AprobadoPorDirigente),
                Cumplida = g.All(r => r.AprobadoPorDirigente)
            })
            .ToList();

        return resumen;
    }

    public async Task<Especialidad?> ObtenerPorIdConRequisitosAsync(Guid id)
    {
        return await _context.Especialidades
            .Include(e => e.Requisitos)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Especialidad> CrearEspecialidadAsync(Especialidad especialidad)
    {
        _context.Especialidades.Add(especialidad);
        await _context.SaveChangesAsync();
        return especialidad;
    }

    public async Task<Requisito> AgregarRequisitoAsync(Guid especialidadId, Requisito requisito)
    {
        requisito.EspecialidadId = especialidadId;
        _context.Requisitos.Add(requisito);
        await _context.SaveChangesAsync();
        return requisito;
    }

    public async Task<List<ScoutConAvanceDto>> ObtenerScoutsConAvancePendienteAsync(Guid unidadId)
    {
        return await _context.Users
            .Where(u => u.UnidadId == unidadId && u.Tipo == "Scout")
            .Where(u => _context.RequisitoCumplidos.Any(rc => rc.ScoutId == u.Id && !rc.AprobadoPorDirigente))
            .Select(u => new ScoutConAvanceDto
            {
                Id = u.Id.ToString(),
                NombreCompleto = u.NombreCompleto,
                Rama = u.Rama
            })
            .ToListAsync();
    }

    public async Task<List<RequisitoConEspecialidadDto>> ObtenerRequisitosPendientesPorScout(string scoutId)
    {
        return await _context.RequisitoCumplidos
            .Where(rc => rc.ScoutId.ToString() == scoutId && rc.AprobadoPorDirigente == false)
            .Include(rc => rc.Requisito)
            .ThenInclude(r => r.Especialidad)
            .Select(rc => new RequisitoConEspecialidadDto
            {
                CumplidoId = rc.Id,
                Texto = rc.Requisito.Texto,
                Tipo = rc.Requisito.Tipo.ToString(),
                EspecialidadNombre = rc.Requisito.Especialidad.Nombre,
                Fecha = rc.Fecha
            })
            .ToListAsync();
    }

    public async Task<List<ResumenScoutEspecialidadesDto>> ObtenerResumenDeEspecialidadesPorScoutAsync(Guid unidadId)
    {
        var scouts = await _context.Users
            .Where(u => u.UnidadId == unidadId && u.Tipo == "Scout")
            .ToListAsync();

        var resumen = new List<ResumenScoutEspecialidadesDto>();

        foreach (var scout in scouts)
        {
            var especialidades = await _context.Especialidades
                .Include(e => e.Requisitos)
                .ToListAsync();

            var scoutId = scout.Id;

            var avances = especialidades.Select(e =>
            {
                var total = e.Requisitos.Count;
                var seleccionados = e.Requisitos.Count(r =>
                    _context.RequisitoCumplidos.Any(rc => rc.RequisitoId == r.Id && rc.ScoutId == scoutId));
                var aprobados = e.Requisitos.Count(r =>
                    _context.RequisitoCumplidos.Any(rc => rc.RequisitoId == r.Id && rc.ScoutId == scoutId && rc.AprobadoPorDirigente));

                return new ResumenEspecialidadDirigenteDto
                {
                    EspecialidadId = e.Id,
                    Nombre = e.Nombre,
                    Seleccionados = seleccionados,
                    Aprobados = aprobados,
                    Cumplida = total > 0 && total == aprobados
                };
            })
            .Where(r => r.Seleccionados > 0)
            .ToList();

            resumen.Add(new ResumenScoutEspecialidadesDto
            {
                ScoutId = scout.Id,
                NombreCompleto = scout.NombreCompleto,
                Especialidades = avances
            });
        }

        return resumen;
    }
}

