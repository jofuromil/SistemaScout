using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendScout.Migrations
{
    /// <inheritdoc />
    public partial class CrearRequisitoCumplido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequisitoCumplidos_Requisitos_RequisitoId",
                table: "RequisitoCumplidos");

            migrationBuilder.DropForeignKey(
                name: "FK_RequisitoCumplidos_Users_ScoutId",
                table: "RequisitoCumplidos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequisitoCumplidos",
                table: "RequisitoCumplidos");

            migrationBuilder.RenameTable(
                name: "RequisitoCumplidos",
                newName: "RequisitoCumplido");

            migrationBuilder.RenameIndex(
                name: "IX_RequisitoCumplidos_ScoutId",
                table: "RequisitoCumplido",
                newName: "IX_RequisitoCumplido_ScoutId");

            migrationBuilder.RenameIndex(
                name: "IX_RequisitoCumplidos_RequisitoId",
                table: "RequisitoCumplido",
                newName: "IX_RequisitoCumplido_RequisitoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequisitoCumplido",
                table: "RequisitoCumplido",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RequisitoCumplido_Requisitos_RequisitoId",
                table: "RequisitoCumplido",
                column: "RequisitoId",
                principalTable: "Requisitos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequisitoCumplido_Users_ScoutId",
                table: "RequisitoCumplido",
                column: "ScoutId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequisitoCumplido_Requisitos_RequisitoId",
                table: "RequisitoCumplido");

            migrationBuilder.DropForeignKey(
                name: "FK_RequisitoCumplido_Users_ScoutId",
                table: "RequisitoCumplido");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequisitoCumplido",
                table: "RequisitoCumplido");

            migrationBuilder.RenameTable(
                name: "RequisitoCumplido",
                newName: "RequisitoCumplidos");

            migrationBuilder.RenameIndex(
                name: "IX_RequisitoCumplido_ScoutId",
                table: "RequisitoCumplidos",
                newName: "IX_RequisitoCumplidos_ScoutId");

            migrationBuilder.RenameIndex(
                name: "IX_RequisitoCumplido_RequisitoId",
                table: "RequisitoCumplidos",
                newName: "IX_RequisitoCumplidos_RequisitoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequisitoCumplidos",
                table: "RequisitoCumplidos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RequisitoCumplidos_Requisitos_RequisitoId",
                table: "RequisitoCumplidos",
                column: "RequisitoId",
                principalTable: "Requisitos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequisitoCumplidos_Users_ScoutId",
                table: "RequisitoCumplidos",
                column: "ScoutId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
