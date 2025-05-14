using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using BackendScout.Models;

namespace BackendScout.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Aquí defines tu proveedor de base de datos:
            optionsBuilder.UseSqlite("Data Source=ScoutDB.db"); // O usa tu cadena real

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
