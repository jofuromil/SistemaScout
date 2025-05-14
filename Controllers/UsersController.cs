using BackendScout.Models;
using BackendScout.Services;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BackendScout.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<User>> RegistrarUsuario([FromBody] User user)
        {
            var nuevoUsuario = await _userService.RegistrarUsuario(user);
            return Ok(nuevoUsuario);
        }

        [HttpGet("todos")]
        public async Task<ActionResult<List<User>>> ObtenerTodos()
        {
            var usuarios = await _userService.ObtenerUsuarios();
            return Ok(usuarios);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, [FromServices] JwtService jwtService)
        {
            var user = await _userService.ValidarLogin(request.Email, request.Password);
            if (user == null)
                return Unauthorized(new { mensaje = "Credenciales inválidas" });

            var token = jwtService.GenerarToken(user);

            return Ok(new
            {
                token,
                user = new
                {
                    user.Id,
                    user.NombreCompleto,
                    user.Tipo,
                    user.Rama
                }
            });
        }

        [HttpPost("unirse-a-unidad")]
        public async Task<IActionResult> UnirseUnidad([FromBody] UnirseUnidadRequest request)
        {
            try
            {
                var unido = await _userService.UnirseUnidad(request.UsuarioId, request.CodigoUnidad);
                return Ok("Te has unido a la unidad correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("salir-de-unidad/{usuarioId}")]
        public async Task<IActionResult> SalirUnidad(Guid usuarioId)
        {
            try
            {
                await _userService.SalirDeUnidad(usuarioId);
                return Ok("Has salido de la unidad.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("eliminar-de-unidad")]
        public async Task<IActionResult> EliminarDeUnidad([FromBody] EliminarRequest request)
        {
            try
            {
                await _userService.EliminarUsuarioDeUnidad(request.DirigenteId, request.UsuarioId);
                return Ok("Usuario eliminado de la unidad.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("miembros-unidad/{dirigenteId}")]
        public async Task<IActionResult> ObtenerMiembros(Guid dirigenteId, [FromQuery] string? tipo)
        {
            try
            {
                var miembros = await _userService.ObtenerMiembrosDeUnidad(dirigenteId, tipo);
                return Ok(miembros);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("exportar-pdf/{dirigenteId}")]
        public async Task<IActionResult> ExportarPdf(Guid dirigenteId, [FromQuery] string? tipo)
        {
            try
            {
                var miembros = await _userService.ObtenerMiembrosDeUnidad(dirigenteId, tipo ?? "");

                var pdf = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(30);
                        page.Header().Text("Lista de miembros de unidad").FontSize(20).Bold().AlignCenter();
                        page.Content().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(120);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                            });

                            // Encabezados
                            table.Cell().Element(CellStyle).Text("Nombre");
                            table.Cell().Element(CellStyle).Text("Correo");
                            table.Cell().Element(CellStyle).Text("Teléfono");
                            table.Cell().Element(CellStyle).Text("Ciudad");
                            table.Cell().Element(CellStyle).Text("Tipo");
                            table.Cell().Element(CellStyle).Text("Rama");

                            // Datos
                            foreach (var u in miembros)
                            {
                                table.Cell().Element(CellStyle).Text(u.NombreCompleto);
                                table.Cell().Element(CellStyle).Text(u.Correo);
                                table.Cell().Element(CellStyle).Text(u.Telefono);
                                table.Cell().Element(CellStyle).Text(u.Ciudad);
                                table.Cell().Element(CellStyle).Text(u.Tipo);
                                table.Cell().Element(CellStyle).Text(u.Rama);
                            }

                            static IContainer CellStyle(IContainer container)
                            {
                                return container
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .PaddingVertical(5)
                                    .PaddingHorizontal(2);
                            }
                        });

                        page.Footer().AlignCenter().Text(txt =>
                        {
                            txt.Span("Generado por Sistema Scout").FontSize(10);
                        });
                    });
                });

                var stream = new MemoryStream();
                pdf.GeneratePdf(stream);
                stream.Position = 0;

                return File(stream.ToArray(), "application/pdf", "miembros_unidad.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
