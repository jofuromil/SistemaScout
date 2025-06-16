using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendScout.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurarRelacionesUsuarioEvento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioEvento_Users_UserId",
                table: "UsuarioEvento");

            migrationBuilder.DropIndex(
                name: "IX_UsuarioEvento_UserId",
                table: "UsuarioEvento");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UsuarioEvento");

            migrationBuilder.AddColumn<int>(
                name: "EventoId1",
                table: "UsuarioEvento",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioEvento_EventoId1",
                table: "UsuarioEvento",
                column: "EventoId1");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioEvento_UsuarioId",
                table: "UsuarioEvento",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioEvento_Eventos_EventoId1",
                table: "UsuarioEvento",
                column: "EventoId1",
                principalTable: "Eventos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioEvento_Users_UsuarioId",
                table: "UsuarioEvento",
                column: "UsuarioId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioEvento_Eventos_EventoId1",
                table: "UsuarioEvento");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioEvento_Users_UsuarioId",
                table: "UsuarioEvento");

            migrationBuilder.DropIndex(
                name: "IX_UsuarioEvento_EventoId1",
                table: "UsuarioEvento");

            migrationBuilder.DropIndex(
                name: "IX_UsuarioEvento_UsuarioId",
                table: "UsuarioEvento");

            migrationBuilder.DropColumn(
                name: "EventoId1",
                table: "UsuarioEvento");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UsuarioEvento",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioEvento_UserId",
                table: "UsuarioEvento",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioEvento_Users_UserId",
                table: "UsuarioEvento",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
