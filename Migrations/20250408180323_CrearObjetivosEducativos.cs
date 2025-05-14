using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendScout.Migrations
{
    /// <inheritdoc />
    public partial class CrearObjetivosEducativos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ObjetivosEducativos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Rama = table.Column<string>(type: "TEXT", nullable: false),
                    EdadMinima = table.Column<int>(type: "INTEGER", nullable: false),
                    EdadMaxima = table.Column<int>(type: "INTEGER", nullable: false),
                    Area = table.Column<string>(type: "TEXT", nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjetivosEducativos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObjetivosSeleccionados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ObjetivoEducativoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FechaSeleccion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Validado = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjetivosSeleccionados", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObjetivosEducativos");

            migrationBuilder.DropTable(
                name: "ObjetivosSeleccionados");
        }
    }
}
