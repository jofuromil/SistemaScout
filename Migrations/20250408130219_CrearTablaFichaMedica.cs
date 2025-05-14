using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendScout.Migrations
{
    /// <inheritdoc />
    public partial class CrearTablaFichaMedica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FichasMedicas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Direccion = table.Column<string>(type: "TEXT", nullable: false),
                    Genero = table.Column<string>(type: "TEXT", nullable: false),
                    TipoSangre = table.Column<string>(type: "TEXT", nullable: false),
                    Alergias = table.Column<string>(type: "TEXT", nullable: false),
                    CondicionesAlimentarias = table.Column<string>(type: "TEXT", nullable: false),
                    Medicamentos = table.Column<string>(type: "TEXT", nullable: false),
                    ObservacionesMedicas = table.Column<string>(type: "TEXT", nullable: false),
                    SeguroSalud = table.Column<string>(type: "TEXT", nullable: false),
                    Colegio = table.Column<string>(type: "TEXT", nullable: true),
                    Curso = table.Column<string>(type: "TEXT", nullable: true),
                    NombrePadre = table.Column<string>(type: "TEXT", nullable: true),
                    TelefonoPadre = table.Column<string>(type: "TEXT", nullable: true),
                    NombreMadre = table.Column<string>(type: "TEXT", nullable: true),
                    TelefonoMadre = table.Column<string>(type: "TEXT", nullable: true),
                    Profesion = table.Column<string>(type: "TEXT", nullable: true),
                    NivelFormacion = table.Column<string>(type: "TEXT", nullable: true),
                    NombreContactoEmergencia = table.Column<string>(type: "TEXT", nullable: true),
                    TelefonoContactoEmergencia = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FichasMedicas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FichasMedicas");
        }
    }
}
