using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendScout.Migrations
{
    /// <inheritdoc />
    public partial class CrearDocumentoEvento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizadoresEvento_Users_UserId1",
                table: "OrganizadoresEvento");

            migrationBuilder.DropIndex(
                name: "IX_OrganizadoresEvento_UserId1",
                table: "OrganizadoresEvento");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "OrganizadoresEvento");

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "UsuariosEvento",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "OrganizadoresEvento",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateTable(
                name: "DocumentosEvento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventoId = table.Column<int>(type: "INTEGER", nullable: false),
                    NombreArchivo = table.Column<string>(type: "TEXT", nullable: false),
                    RutaArchivo = table.Column<string>(type: "TEXT", nullable: false),
                    TipoMime = table.Column<string>(type: "TEXT", nullable: false),
                    FechaSubida = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SubidoPorId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentosEvento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentosEvento_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentosEvento_Users_SubidoPorId",
                        column: x => x.SubidoPorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizadoresEvento_UserId",
                table: "OrganizadoresEvento",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosEvento_EventoId",
                table: "DocumentosEvento",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosEvento_SubidoPorId",
                table: "DocumentosEvento",
                column: "SubidoPorId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizadoresEvento_Users_UserId",
                table: "OrganizadoresEvento",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizadoresEvento_Users_UserId",
                table: "OrganizadoresEvento");

            migrationBuilder.DropTable(
                name: "DocumentosEvento");

            migrationBuilder.DropIndex(
                name: "IX_OrganizadoresEvento_UserId",
                table: "OrganizadoresEvento");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "UsuariosEvento");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "OrganizadoresEvento",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "OrganizadoresEvento",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OrganizadoresEvento_UserId1",
                table: "OrganizadoresEvento",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizadoresEvento_Users_UserId1",
                table: "OrganizadoresEvento",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
