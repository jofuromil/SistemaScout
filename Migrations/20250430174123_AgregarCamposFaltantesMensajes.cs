using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendScout.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCamposFaltantesMensajes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Titulo",
                table: "Mensajes",
                newName: "Fecha");

            migrationBuilder.RenameColumn(
                name: "FechaEnvio",
                table: "Mensajes",
                newName: "DirigenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_DirigenteId",
                table: "Mensajes",
                column: "DirigenteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mensajes_Users_DirigenteId",
                table: "Mensajes",
                column: "DirigenteId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mensajes_Users_DirigenteId",
                table: "Mensajes");

            migrationBuilder.DropIndex(
                name: "IX_Mensajes_DirigenteId",
                table: "Mensajes");

            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "Mensajes",
                newName: "Titulo");

            migrationBuilder.RenameColumn(
                name: "DirigenteId",
                table: "Mensajes",
                newName: "FechaEnvio");
        }
    }
}
