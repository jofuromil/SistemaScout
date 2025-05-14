using BackendScout.Data;
using BackendScout.Models;
using BackendScout.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BackendScout.Services
{
    public class UnidadService
    {
        private readonly AppDbContext _context;

        public UnidadService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Unidad> CrearUnidad(CrearUnidadRequest request)
        {
            // Validar distrito
            if (!GruposPorDistrito.DistritoExiste(request.Distrito))
                throw new Exception("El distrito seleccionado no es válido.");

            // Validar grupo dentro del distrito
            if (!GruposPorDistrito.GrupoValido(request.Distrito, request.GrupoScout))
                throw new Exception("El grupo scout no pertenece a ese distrito o no está habilitado.");

            // Validar que el dirigente exista
            var dirigente = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.DirigenteId);
            if (dirigente == null)
                throw new Exception("El dirigente no fue encontrado.");

            // Validar que sea dirigente
            if (dirigente.Tipo.ToLower() != "dirigente")
                throw new Exception("Solo un dirigente puede crear una unidad.");

            // Validar que no esté ya en una unidad
            if (dirigente.UnidadId != null)
                throw new Exception("Este dirigente ya está en una unidad. Debe salir antes de crear una nueva.");

            // Crear nueva unidad
            var nueva = new Unidad
            {
                Id = Guid.NewGuid(),
                Nombre = request.Nombre,
                Rama = request.Rama,
                GrupoScout = request.GrupoScout,
                Distrito = request.Distrito,
                DirigenteId = request.DirigenteId,
                CodigoUnidad = Guid.NewGuid().ToString().Substring(0, 8).ToUpper()
            };

            _context.Unidades.Add(nueva);
            await _context.SaveChangesAsync();

            // Asociar el dirigente a la unidad
            dirigente.UnidadId = nueva.Id;
            await _context.SaveChangesAsync();

            return nueva;
        }

        public async Task<List<Unidad>> ObtenerUnidades()
        {
            return await _context.Unidades.ToListAsync();
        }
    }
}
