using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendScout.Migrations
{
    /// <inheritdoc />
    public partial class CrearModuloEventos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TiposEvento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposEvento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    TipoEventoId = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: false),
                    ImagenUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Nivel = table.Column<string>(type: "TEXT", nullable: false),
                    OrganizadorUnidadId = table.Column<int>(type: "INTEGER", nullable: true),
                    OrganizadorGrupoId = table.Column<int>(type: "INTEGER", nullable: true),
                    OrganizadorDistritoId = table.Column<int>(type: "INTEGER", nullable: true),
                    RamasDestino = table.Column<string>(type: "TEXT", nullable: false),
                    CupoMaximo = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eventos_TiposEvento_TipoEventoId",
                        column: x => x.TipoEventoId,
                        principalTable: "TiposEvento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizadoresEvento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventoId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId1 = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizadoresEvento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizadoresEvento_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizadoresEvento_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosEvento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventoId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId1 = table.Column<Guid>(type: "TEXT", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", nullable: false),
                    TipoParticipacion = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosEvento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuariosEvento_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuariosEvento_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_TipoEventoId",
                table: "Eventos",
                column: "TipoEventoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizadoresEvento_EventoId",
                table: "OrganizadoresEvento",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizadoresEvento_UserId1",
                table: "OrganizadoresEvento",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosEvento_EventoId",
                table: "UsuariosEvento",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosEvento_UserId1",
                table: "UsuariosEvento",
                column: "UserId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizadoresEvento");

            migrationBuilder.DropTable(
                name: "UsuariosEvento");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "TiposEvento");
        }
    }
}
