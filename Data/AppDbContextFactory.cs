using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using BackendScout.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;


namespace BackendScout.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Aquí defines tu proveedor de base de datos:
            optionsBuilder.UseNpgsql("Host=gondola.proxy.rlwy.net;Port=51801;Database=railway;Username=postgres;Password=LgvuqZTbFDXsMPWiImNvDhVtrbPnvuDE;Ssl Mode=Require;Trust Server Certificate=true");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
