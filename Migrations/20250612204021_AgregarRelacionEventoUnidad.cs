using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendScout.Migrations
{
    /// <inheritdoc />
    public partial class AgregarRelacionEventoUnidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Eventos_OrganizadorUnidadId",
                table: "Eventos",
                column: "OrganizadorUnidadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Unidades_OrganizadorUnidadId",
                table: "Eventos",
                column: "OrganizadorUnidadId",
                principalTable: "Unidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Unidades_OrganizadorUnidadId",
                table: "Eventos");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_OrganizadorUnidadId",
                table: "Eventos");
        }
    }
}
