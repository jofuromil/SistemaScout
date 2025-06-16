using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BackendScout.Migrations
{
    /// <inheritdoc />
    public partial class ResetUUIDFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Especialidades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Rama = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especialidades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FichasMedicas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Direccion = table.Column<string>(type: "text", nullable: false),
                    Genero = table.Column<string>(type: "text", nullable: false),
                    TipoSangre = table.Column<string>(type: "text", nullable: false),
                    Alergias = table.Column<string>(type: "text", nullable: false),
                    CondicionesAlimentarias = table.Column<string>(type: "text", nullable: false),
                    Medicamentos = table.Column<string>(type: "text", nullable: false),
                    ObservacionesMedicas = table.Column<string>(type: "text", nullable: false),
                    SeguroSalud = table.Column<string>(type: "text", nullable: false),
                    Colegio = table.Column<string>(type: "text", nullable: true),
                    Curso = table.Column<string>(type: "text", nullable: true),
                    NombrePadre = table.Column<string>(type: "text", nullable: true),
                    TelefonoPadre = table.Column<string>(type: "text", nullable: true),
                    NombreMadre = table.Column<string>(type: "text", nullable: true),
                    TelefonoMadre = table.Column<string>(type: "text", nullable: true),
                    Profesion = table.Column<string>(type: "text", nullable: true),
                    NivelFormacion = table.Column<string>(type: "text", nullable: true),
                    NombreContactoEmergencia = table.Column<string>(type: "text", nullable: true),
                    TelefonoContactoEmergencia = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FichasMedicas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObjetivosEducativos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Rama = table.Column<string>(type: "text", nullable: false),
                    EdadMinima = table.Column<int>(type: "integer", nullable: false),
                    EdadMaxima = table.Column<int>(type: "integer", nullable: false),
                    Area = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    NivelProgresion = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjetivosEducativos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Unidades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Rama = table.Column<string>(type: "text", nullable: false),
                    GrupoScout = table.Column<string>(type: "text", nullable: false),
                    Distrito = table.Column<string>(type: "text", nullable: false),
                    CodigoUnidad = table.Column<string>(type: "text", nullable: false),
                    DirigenteId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unidades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Requisitos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EspecialidadId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Texto = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requisitos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requisitos_Especialidades_EspecialidadId",
                        column: x => x.EspecialidadId,
                        principalTable: "Especialidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    ImagenUrl = table.Column<string>(type: "text", nullable: true),
                    Nivel = table.Column<string>(type: "text", nullable: false),
                    OrganizadorUnidadId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrganizadorGrupoId = table.Column<int>(type: "integer", nullable: true),
                    OrganizadorDistritoId = table.Column<int>(type: "integer", nullable: true),
                    RamasDestino = table.Column<List<string>>(type: "text[]", nullable: false),
                    CupoMaximo = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eventos_Unidades_OrganizadorUnidadId",
                        column: x => x.OrganizadorUnidadId,
                        principalTable: "Unidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NombreCompleto = table.Column<string>(type: "text", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Telefono = table.Column<string>(type: "text", nullable: false),
                    Correo = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Ciudad = table.Column<string>(type: "text", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    Rama = table.Column<string>(type: "text", nullable: false),
                    UnidadId = table.Column<Guid>(type: "uuid", nullable: true),
                    Direccion = table.Column<string>(type: "text", nullable: true),
                    InstitucionEducativa = table.Column<string>(type: "text", nullable: true),
                    NivelEstudios = table.Column<string>(type: "text", nullable: true),
                    Genero = table.Column<string>(type: "text", nullable: true),
                    Profesion = table.Column<string>(type: "text", nullable: true),
                    Ocupacion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Unidades_UnidadId",
                        column: x => x.UnidadId,
                        principalTable: "Unidades",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DocumentosEvento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventoId = table.Column<int>(type: "integer", nullable: false),
                    NombreArchivo = table.Column<string>(type: "text", nullable: false),
                    RutaArchivo = table.Column<string>(type: "text", nullable: false),
                    TipoMime = table.Column<string>(type: "text", nullable: false),
                    FechaSubida = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SubidoPorId = table.Column<Guid>(type: "uuid", nullable: false),
                    EsEnlaceExterno = table.Column<bool>(type: "boolean", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Mensajes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Contenido = table.Column<string>(type: "text", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UnidadId = table.Column<Guid>(type: "uuid", nullable: false),
                    DirigenteId = table.Column<Guid>(type: "uuid", nullable: false),
                    RutaImagen = table.Column<string>(type: "text", nullable: true),
                    RutaArchivo = table.Column<string>(type: "text", nullable: true),
                    ExpiraEl = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_Mensajes_Users_DirigenteId",
                        column: x => x.DirigenteId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MensajesEvento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventoId = table.Column<int>(type: "integer", nullable: false),
                    RemitenteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Contenido = table.Column<string>(type: "text", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                name: "ObjetivosSeleccionados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    ObjetivoEducativoId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaSeleccion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Validado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjetivosSeleccionados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjetivosSeleccionados_ObjetivosEducativos_ObjetivoEducativ~",
                        column: x => x.ObjetivoEducativoId,
                        principalTable: "ObjetivosEducativos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObjetivosSeleccionados_Users_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizadoresEvento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventoId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
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
                        name: "FK_OrganizadoresEvento_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaGeneracion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Usado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordResetCodes_Users_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequisitoCumplido",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequisitoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScoutId = table.Column<Guid>(type: "uuid", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AprobadoPorDirigente = table.Column<bool>(type: "boolean", nullable: false),
                    FechaAprobacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisitoCumplido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequisitoCumplido_Requisitos_RequisitoId",
                        column: x => x.RequisitoId,
                        principalTable: "Requisitos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequisitoCumplido_Users_ScoutId",
                        column: x => x.ScoutId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioEvento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventoId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    TipoParticipacion = table.Column<string>(type: "text", nullable: false),
                    EventoId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioEvento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioEvento_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioEvento_Eventos_EventoId1",
                        column: x => x.EventoId1,
                        principalTable: "Eventos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UsuarioEvento_Users_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MensajesEventoDestinatarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MensajeEventoId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false)
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
                name: "IX_DocumentosEvento_EventoId",
                table: "DocumentosEvento",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosEvento_SubidoPorId",
                table: "DocumentosEvento",
                column: "SubidoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_OrganizadorUnidadId",
                table: "Eventos",
                column: "OrganizadorUnidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_DirigenteId",
                table: "Mensajes",
                column: "DirigenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_UnidadId",
                table: "Mensajes",
                column: "UnidadId");

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

            migrationBuilder.CreateIndex(
                name: "IX_ObjetivosSeleccionados_ObjetivoEducativoId",
                table: "ObjetivosSeleccionados",
                column: "ObjetivoEducativoId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjetivosSeleccionados_UsuarioId",
                table: "ObjetivosSeleccionados",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizadoresEvento_EventoId",
                table: "OrganizadoresEvento",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizadoresEvento_UserId",
                table: "OrganizadoresEvento",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetCodes_UsuarioId",
                table: "PasswordResetCodes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitoCumplido_RequisitoId",
                table: "RequisitoCumplido",
                column: "RequisitoId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitoCumplido_ScoutId",
                table: "RequisitoCumplido",
                column: "ScoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Requisitos_EspecialidadId",
                table: "Requisitos",
                column: "EspecialidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UnidadId",
                table: "Users",
                column: "UnidadId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioEvento_EventoId",
                table: "UsuarioEvento",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioEvento_EventoId1",
                table: "UsuarioEvento",
                column: "EventoId1");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioEvento_UsuarioId",
                table: "UsuarioEvento",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentosEvento");

            migrationBuilder.DropTable(
                name: "FichasMedicas");

            migrationBuilder.DropTable(
                name: "Mensajes");

            migrationBuilder.DropTable(
                name: "MensajesEventoDestinatarios");

            migrationBuilder.DropTable(
                name: "ObjetivosSeleccionados");

            migrationBuilder.DropTable(
                name: "OrganizadoresEvento");

            migrationBuilder.DropTable(
                name: "PasswordResetCodes");

            migrationBuilder.DropTable(
                name: "RequisitoCumplido");

            migrationBuilder.DropTable(
                name: "UsuarioEvento");

            migrationBuilder.DropTable(
                name: "MensajesEvento");

            migrationBuilder.DropTable(
                name: "ObjetivosEducativos");

            migrationBuilder.DropTable(
                name: "Requisitos");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Especialidades");

            migrationBuilder.DropTable(
                name: "Unidades");
        }
    }
}
