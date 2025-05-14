using BackendScout.Data;
using BackendScout.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendScout.Services
{
    public class FichaMedicaService
    {
        private readonly AppDbContext _context;

        public FichaMedicaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FichaMedica> GuardarFicha(FichaMedica ficha)
        {
            var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Id == ficha.UsuarioId);

            if (usuario == null || usuario.UnidadId == null)
                throw new Exception("Debes estar en una unidad para llenar tu ficha médica.");

// Validaciones básicas
if (string.IsNullOrWhiteSpace(ficha.Direccion))
    throw new Exception("La dirección es obligatoria.");

if (string.IsNullOrWhiteSpace(ficha.Genero))
    throw new Exception("El género es obligatorio.");

if (!string.IsNullOrWhiteSpace(ficha.TipoSangre) &&
    !new[] { "O+", "O-", "A+", "A-", "B+", "B-", "AB+", "AB-" }
        .Contains(ficha.TipoSangre.ToUpper()))
{
    throw new Exception("Tipo de sangre no válido.");
}

if (!string.IsNullOrWhiteSpace(ficha.TelefonoPadre) && ficha.TelefonoPadre.Length < 7)
    throw new Exception("El teléfono del padre debe tener al menos 7 dígitos.");

if (!string.IsNullOrWhiteSpace(ficha.TelefonoMadre) && ficha.TelefonoMadre.Length < 7)
    throw new Exception("El teléfono de la madre debe tener al menos 7 dígitos.");

if (!string.IsNullOrWhiteSpace(ficha.TelefonoContactoEmergencia) && ficha.TelefonoContactoEmergencia.Length < 7)
    throw new Exception("El teléfono del contacto de emergencia debe tener al menos 7 dígitos.");

            var existente = await _context.FichasMedicas.FirstOrDefaultAsync(f => f.UsuarioId == ficha.UsuarioId);

            if (existente != null)
            {
                // Actualiza campos
                _context.Entry(existente).CurrentValues.SetValues(ficha);
            }
            else
            {
                await _context.FichasMedicas.AddAsync(ficha);
            }

            await _context.SaveChangesAsync();
            return ficha;
        }

        public async Task<FichaMedica?> ObtenerFicha(Guid usuarioId)
        {
            return await _context.FichasMedicas.FirstOrDefaultAsync(f => f.UsuarioId == usuarioId);
        }
        public async Task<FichaMedica?> VerFichaComoDirigente(Guid dirigenteId, Guid usuarioId)
    {
    var dirigente = await _context.Users.FirstOrDefaultAsync(u => u.Id == dirigenteId);

    if (dirigente == null || dirigente.Tipo.ToLower() != "dirigente")
        throw new Exception("Solo los dirigentes pueden acceder a esta información.");

    var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Id == usuarioId);
    if (usuario == null)
        throw new Exception("Usuario no encontrado.");

    if (usuario.UnidadId != dirigente.UnidadId)
        throw new Exception("Este usuario no pertenece a tu unidad.");

    return await _context.FichasMedicas.FirstOrDefaultAsync(f => f.UsuarioId == usuarioId);
    }
    }
    

}
