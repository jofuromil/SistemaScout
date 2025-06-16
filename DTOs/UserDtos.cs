namespace BackendScout.Dtos 
{
    public class RegisterUserDto
    {
        public string NombreCompleto { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
        public string Ciudad { get; set; }
        public string Tipo { get; set; } // "Scout" o "Dirigente"
        public string Rama { get; set; } // Lobatos, Exploradores, Pioneros, Rovers
    }

    public class LoginUserDto
    {
        public string Correo { get; set; }
        public string Password { get; set; }
    }

    public class UserDto
    {
        public Guid Id { get; set; }
        public string NombreCompleto { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Ciudad { get; set; }
        public string Tipo { get; set; }
        public string Rama { get; set; }

        // Campos adicionales del perfil
        public string? Direccion { get; set; }
        public string? InstitucionEducativa { get; set; }
        public string? NivelEstudios { get; set; }
        public string? Genero { get; set; }
        public string? Profesion { get; set; }   // SOLO dirigentes
        public string? Ocupacion { get; set; }   // SOLO dirigentes

    }
}
