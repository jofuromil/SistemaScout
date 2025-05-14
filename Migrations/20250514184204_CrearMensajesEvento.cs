using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendScout.Migrations
{
    /// <inheritdoc />
    public partial class CrearMensajesEvento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MensajesEvento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventoId = table.Column<int>(type: "INTEGER", nullable: false),
                    RemitenteId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Contenido = table.Column<string>(type: "TEXT", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MensajesEvento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MensajesEvento_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MensajesEvento_Users_RemitenteId",
                        column: x => x.RemitenteId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MensajesEventoDestinatarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MensajeEventoId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MensajesEventoDestinatarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MensajesEventoDestinatarios_MensajesEvento_MensajeEventoId",
                        column: x => x.MensajeEventoId,
                        principalTable: "MensajesEvento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MensajesEventoDestinatarios_Users_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MensajesEvento_EventoId",
                table: "MensajesEvento",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_MensajesEvento_RemitenteId",
                table: "MensajesEvento",
                column: "RemitenteId");

            migrationBuilder.CreateIndex(
                name: "IX_MensajesEventoDestinatarios_MensajeEventoId",
                table: "MensajesEventoDestinatarios",
                column: "MensajeEventoId");

            migrationBuilder.CreateIndex(
                name: "IX_MensajesEventoDestinatarios_UsuarioId",
                table: "MensajesEventoDestinatarios",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MensajesEventoDestinatarios");

            migrationBuilder.DropTable(
                name: "MensajesEvento");
        }
    }
}
