using System;

namespace BackendScout.DTOs
{
    public class RegistroRequest
    {
        public string NombreCompleto { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public string Correo { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
