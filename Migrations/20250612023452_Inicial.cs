using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendScout.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Especialidades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Rama = table.Column<string>(type: "TEXT", nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especialidades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: false),
                    ImagenUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Nivel = table.Column<string>(type: "TEXT", nullable: false),
                    OrganizadorUnidadId = table.Column<Guid>(type: "TEXT", nullable: true),
                    OrganizadorGrupoId = table.Column<int>(type: "INTEGER", nullable: true),
                    OrganizadorDistritoId = table.Column<int>(type: "INTEGER", nullable: true),
                    RamasDestino = table.Column<string>(type: "TEXT", nullable: false),
                    CupoMaximo = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "ObjetivosEducativos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Rama = table.Column<string>(type: "TEXT", nullable: false),
                    EdadMinima = table.Column<int>(type: "INTEGER", nullable: false),
                    EdadMaxima = table.Column<int>(type: "INTEGER", nullable: false),
                    Area = table.Column<string>(type: "TEXT", nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: false),
                    NivelProgresion = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjetivosEducativos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Unidades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Rama = table.Column<string>(type: "TEXT", nullable: false),
                    GrupoScout = table.Column<string>(type: "TEXT", nullable: false),
                    Distrito = table.Column<string>(type: "TEXT", nullable: false),
                    CodigoUnidad = table.Column<string>(type: "TEXT", nullable: false),
                    DirigenteId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unidades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Requisitos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EspecialidadId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    Texto = table.Column<string>(type: "TEXT", nullable: false)
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
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    NombreCompleto = table.Column<string>(type: "TEXT", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Telefono = table.Column<string>(type: "TEXT", nullable: false),
                    Correo = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Ciudad = table.Column<string>(type: "TEXT", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    Rama = table.Column<string>(type: "TEXT", nullable: false),
                    UnidadId = table.Column<Guid>(type: "TEXT", nullable: true)
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventoId = table.Column<int>(type: "INTEGER", nullable: false),
                    NombreArchivo = table.Column<string>(type: "TEXT", nullable: false),
                    RutaArchivo = table.Column<string>(type: "TEXT", nullable: false),
                    TipoMime = table.Column<string>(type: "TEXT", nullable: false),
                    FechaSubida = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SubidoPorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EsEnlaceExterno = table.Column<bool>(type: "INTEGER", nullable: false)
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
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Contenido = table.Column<string>(type: "TEXT", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UnidadId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DirigenteId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RutaImagen = table.Column<string>(type: "TEXT", nullable: true),
                    RutaArchivo = table.Column<string>(type: "TEXT", nullable: true),
                    ExpiraEl = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                name: "ObjetivosSeleccionados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ObjetivoEducativoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FechaSeleccion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Validado = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjetivosSeleccionados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjetivosSeleccionados_ObjetivosEducativos_ObjetivoEducativoId",
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventoId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
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
                name: "RequisitoCumplidos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RequisitoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ScoutId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AprobadoPorDirigente = table.Column<bool>(type: "INTEGER", nullable: false),
                    FechaAprobacion = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisitoCumplidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequisitoCumplidos_Requisitos_RequisitoId",
                        column: x => x.RequisitoId,
                        principalTable: "Requisitos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequisitoCumplidos_Users_ScoutId",
                        column: x => x.ScoutId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioEvento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventoId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", nullable: false),
                    TipoParticipacion = table.Column<string>(type: "TEXT", nullable: false)
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
                        name: "FK_UsuarioEvento_Users_UserId",
                        column: x => x.UserId,
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
                name: "IX_DocumentosEvento_EventoId",
                table: "DocumentosEvento",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosEvento_SubidoPorId",
                table: "DocumentosEvento",
                column: "SubidoPorId");

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
                name: "IX_RequisitoCumplidos_RequisitoId",
                table: "RequisitoCumplidos",
                column: "RequisitoId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitoCumplidos_ScoutId",
                table: "RequisitoCumplidos",
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
                name: "IX_UsuarioEvento_UserId",
                table: "UsuarioEvento",
                column: "UserId");
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
                name: "RequisitoCumplidos");

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
