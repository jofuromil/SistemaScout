using System;

namespace BackendScout.DTOs
{
    public class RegistroRequest
    {
        public string NombreCompleto { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
    }
}