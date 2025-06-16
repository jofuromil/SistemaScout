using System;
using System.Linq;
using System.Threading.Tasks;
using BackendScout.Data;
using BackendScout.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendScout.Services
{
    public class PasswordResetService
    {
        private readonly AppDbContext _context;

        public PasswordResetService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerarCodigoAsync(Guid usuarioId)
        {
            var codigo = GenerarCodigoUnico();

            var registro = new PasswordResetCode
            {
                Codigo = codigo,
                UsuarioId = usuarioId,
                FechaGeneracion = DateTime.UtcNow,
                Usado = false
            };

            _context.PasswordResetCodes.Add(registro);
            await _context.SaveChangesAsync();

            return codigo;
        }

        public async Task<bool> ValidarCodigoAsync(string codigo)
        {
            var registro = await _context.PasswordResetCodes
                .FirstOrDefaultAsync(c => c.Codigo == codigo && !c.Usado);

            if (registro == null) return false;

            var expirado = DateTime.UtcNow - registro.FechaGeneracion > TimeSpan.FromHours(1);
            return !expirado;
        }

        public async Task<Guid?> ObtenerUsuarioPorCodigoAsync(string codigo)
        {
            var registro = await _context.PasswordResetCodes
                .FirstOrDefaultAsync(c => c.Codigo == codigo && !c.Usado);

            if (registro == null) return null;

            var expirado = DateTime.UtcNow - registro.FechaGeneracion > TimeSpan.FromHours(1);
            return expirado ? null : registro.UsuarioId;
        }

        public async Task MarcarComoUsadoAsync(string codigo)
        {
            var registro = await _context.PasswordResetCodes
                .FirstOrDefaultAsync(c => c.Codigo == codigo);

            if (registro != null)
            {
                registro.Usado = true;
                await _context.SaveChangesAsync();
            }
        }

        private string GenerarCodigoUnico()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
