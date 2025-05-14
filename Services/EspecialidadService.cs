using BackendScout.Data;
using BackendScout.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendScout.Services
{
    public class EspecialidadService
    {
        private readonly AppDbContext _context;

        public EspecialidadService(AppDbContext context)
        {
            _context = context;
        }

        // 1. Obtener todas las especialidades de una rama
        public async Task<List<Especialidad>> ObtenerPorRamaAsync(string rama)
        {
            return await _context.Especialidades
                .Include(e => e.Requisitos)
                .Where(e => e.Rama.ToLower() == rama.ToLower())
                .ToListAsync();
        }

        // 2. Obtener una especialidad por su Id
        public async Task<Especialidad?> ObtenerPorIdAsync(Guid id)
        {
            return await _context.Especialidades
                .Include(e => e.Requisitos)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // 3. Crear una nueva especialidad
        public async Task<Especialidad> CrearEspecialidadAsync(Especialidad especialidad)
        {
            _context.Especialidades.Add(especialidad);
            await _context.SaveChangesAsync();
            return especialidad;
        }

        // 4. Agregar un requisito a una especialidad existente
        public async Task<Requisito> AgregarRequisitoAsync(Guid especialidadId, Requisito requisito)
        {
            var esp = await _context.Especialidades.FindAsync(especialidadId);
            if (esp == null) throw new Exception("Especialidad no encontrada.");

            requisito.EspecialidadId = especialidadId;
            _context.Requisitos.Add(requisito);
            await _context.SaveChangesAsync();
            return requisito;
        }
    }
}
