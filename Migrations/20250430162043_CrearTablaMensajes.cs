using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendScout.Migrations
{
    /// <inheritdoc />
    public partial class CrearTablaMensajes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mensajes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UnidadId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Titulo = table.Column<string>(type: "TEXT", nullable: false),
                    Contenido = table.Column<string>(type: "TEXT", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensajes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mensajes_Unidades_UnidadId",
                        column: x => x.UnidadId,
                        principalTable: "Unidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ObjetivosSeleccionados_ObjetivoEducativoId",
                table: "ObjetivosSeleccionados",
                column: "ObjetivoEducativoId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjetivosSeleccionados_UsuarioId",
                table: "ObjetivosSeleccionados",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_UnidadId",
                table: "Mensajes",
                column: "UnidadId");

            migrationBuilder.AddForeignKey(
                name: "FK_ObjetivosSeleccionados_ObjetivosEducativos_ObjetivoEducativoId",
                table: "ObjetivosSeleccionados",
                column: "ObjetivoEducativoId",
                principalTable: "ObjetivosEducativos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ObjetivosSeleccionados_Users_UsuarioId",
                table: "ObjetivosSeleccionados",
                column: "UsuarioId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ObjetivosSeleccionados_ObjetivosEducativos_ObjetivoEducativoId",
                table: "ObjetivosSeleccionados");

            migrationBuilder.DropForeignKey(
                name: "FK_ObjetivosSeleccionados_Users_UsuarioId",
                table: "ObjetivosSeleccionados");

            migrationBuilder.DropTable(
                name: "Mensajes");

            migrationBuilder.DropIndex(
                name: "IX_ObjetivosSeleccionados_ObjetivoEducativoId",
                table: "ObjetivosSeleccionados");

            migrationBuilder.DropIndex(
                name: "IX_ObjetivosSeleccionados_UsuarioId",
                table: "ObjetivosSeleccionados");
        }
    }
}
