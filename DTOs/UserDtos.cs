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
}
