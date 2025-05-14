namespace BackendScout.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string NombreCompleto { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Ciudad { get; set; }
        public string Tipo { get; set; } // "Scout" o "Dirigente"
        public string Rama { get; set; } // Lobatos, Exploradores, Pioneros, Rovers
        public Guid? UnidadId { get; set; }  // Permite que un usuario esté (o no) en una unidad
        public Unidad? Unidad { get; set; }

    }
}
