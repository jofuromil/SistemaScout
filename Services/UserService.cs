using BackendScout.Data;
using BackendScout.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendScout.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> RegistrarUsuario(User nuevo)
        {
            int edad = CalcularEdad(nuevo.FechaNacimiento);

            if (nuevo.Tipo == "Scout")
            {
                if (edad >= 6 && edad <= 10)
                    nuevo.Rama = "Lobatos";
                else if (edad >= 11 && edad <= 14)
                    nuevo.Rama = "Exploradores";
                else if (edad >= 15 && edad <= 17)
                    nuevo.Rama = "Pioneros";
                else if (edad >= 18 && edad <= 21)
                    nuevo.Rama = "Rovers";
                else
                    nuevo.Rama = "Sin Rama";
            }
            else if (nuevo.Tipo == "Dirigente")
            {
                if (edad < 18)
                    throw new Exception("Un dirigente no puede tener menos de 18 años.");
                nuevo.Rama = "Dirigente";
            }

            _context.Users.Add(nuevo);
            await _context.SaveChangesAsync();

            return nuevo;
        }

        public async Task<List<User>> ObtenerUsuarios()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> ValidarLogin(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Correo == email && u.Password == password);
        }
        
        private int CalcularEdad(DateTime fechaNacimiento)
        {
            var hoy = DateTime.Today;
            var edad = hoy.Year - fechaNacimiento.Year;
            if (fechaNacimiento.Date > hoy.AddYears(-edad)) edad--;
            return edad;
        }
        public async Task<bool> UnirseUnidad(Guid usuarioId, string codigoUnidad)
{
    var unidad = await _context.Unidades.FirstOrDefaultAsync(u => u.CodigoUnidad == codigoUnidad);
    if (unidad == null)
        throw new Exception("Código de unidad no válido.");

    var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Id == usuarioId);
    if (usuario == null)
        throw new Exception("Usuario no encontrado.");

    if (usuario.UnidadId != null)
        throw new Exception("Ya estás en una unidad. Debes salir antes de unirte a otra.");

    // Validación de rama
    if (usuario.Rama.ToLower() != unidad.Rama.ToLower())
        throw new Exception("No puedes unirte a esta unidad porque no corresponde a tu rama.");

    usuario.UnidadId = unidad.Id;
    await _context.SaveChangesAsync();
    return true;
}

public async Task<bool> SalirDeUnidad(Guid usuarioId)
{
    var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Id == usuarioId);
    if (usuario == null)
        throw new Exception("Usuario no encontrado.");

    if (usuario.UnidadId == null)
        throw new Exception("No estás en ninguna unidad.");

    usuario.UnidadId = null;
    await _context.SaveChangesAsync();
    return true;
}
public async Task<bool> EliminarUsuarioDeUnidad(Guid dirigenteId, Guid usuarioAEliminarId)
{
    var dirigente = await _context.Users.FirstOrDefaultAsync(u => u.Id == dirigenteId);
    if (dirigente == null || dirigente.Tipo.ToLower() != "dirigente")
        throw new Exception("Solo los dirigentes pueden eliminar miembros.");

    var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Id == usuarioAEliminarId);
    if (usuario == null)
        throw new Exception("El usuario a eliminar no fue encontrado.");

    if (dirigente.Id == usuario.Id)
        throw new Exception("No puedes eliminarte a ti mismo.");

    if (dirigente.UnidadId == null || dirigente.UnidadId != usuario.UnidadId)
        throw new Exception("Solo puedes eliminar usuarios de tu misma unidad.");

    usuario.UnidadId = null;
    await _context.SaveChangesAsync();
    return true;
}
public async Task<List<User>> ObtenerMiembrosDeUnidad(Guid dirigenteId, string? tipo = null)
{
    var dirigente = await _context.Users.FirstOrDefaultAsync(u => u.Id == dirigenteId);

    if (dirigente == null)
        throw new Exception("Dirigente no encontrado.");

    if (dirigente.Tipo.ToLower() != "dirigente")
        throw new Exception("Solo los dirigentes pueden ver miembros de la unidad.");

    if (dirigente.UnidadId == null)
        throw new Exception("No estás en ninguna unidad.");

    var query = _context.Users.Where(u => u.UnidadId == dirigente.UnidadId);

    if (!string.IsNullOrEmpty(tipo))
    {
        query = query.Where(u => u.Tipo.ToLower() == tipo.ToLower());
    }

    return await query.ToListAsync();
}


    }
}



