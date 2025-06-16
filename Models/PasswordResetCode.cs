using System;
using BackendScout.Models;

namespace BackendScout.Models
{
    public class PasswordResetCode
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public Guid UsuarioId { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public bool Usado { get; set; } = false;

        public User Usuario { get; set; } = null!;
    }
}
