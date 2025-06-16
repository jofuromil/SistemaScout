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

        // 🔽 Nuevos campos de ficha personal
        public string? Direccion { get; set; }
        public string? InstitucionEducativa { get; set; } // Colegio o universidad
        public string? NivelEstudios { get; set; } // Primaria, Secundaria, Universitario, etc.
        public string? Genero { get; set; }
        public string? Profesion { get; set; }       // SOLO Dirigentes
        public string? Ocupacion { get; set; }       // SOLO Dirigentes

    }
}
