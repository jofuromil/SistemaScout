using BackendScout.Models;
using BackendScout.Services;
using BackendScout.DTOs;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BackendScout.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;
        private readonly JwtService _jwtService;

        public UsersController(UserService userService, AuthService authService, JwtService jwtService)
        {
            _userService = userService;
            _authService = authService;
            _jwtService = jwtService;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult> RegistrarUsuario([FromBody] RegistroRequest request)
        {
            if (_userService.ObtenerUsuarioPorCorreo(request.Correo) != null)
                return BadRequest(new { mensaje = "Ya existe un usuario con ese correo." });

            var user = new User
            {
                NombreCompleto = request.NombreCompleto,
                FechaNacimiento = request.FechaNacimiento,
                Correo = request.Correo,
                Password = _authService.HashPassword(request.Password),
                Telefono = string.Empty,
                Ciudad = string.Empty,
                Tipo = "Scout", // valor por defecto
                Rama = string.Empty,
                UnidadId = null
            };

            var nuevoUsuario = await _userService.RegistrarUsuario(user);
            var token = _authService.GenerateJwtToken(nuevoUsuario);

            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var user = await _userService.ValidarLogin(dto.Correo, dto.Password);
            if (user == null)
    return Unauthorized(new { mensaje = "El correo no está registrado." });

if (!_authService.VerifyPassword(dto.Password, user.Password))
{
    // Validación adicional para detectar contraseñas antiguas no hasheadas
    if (!user.Password.StartsWith("$2"))
    {
        return Unauthorized(new
        {
            mensaje = "Tu cuenta fue creada antes de una actualización del sistema. Por favor, crea una nueva cuenta."
        });
    }

    return Unauthorized(new { mensaje = "Contraseña incorrecta." });
}

            var token = _authService.GenerateJwtToken(user);

            return Ok(new
            {
                token,
                user = new
                {
                    user.Id,
                    user.NombreCompleto,
                    user.Tipo,
                    user.Rama,
                    unidadId = user.UnidadId
                }
            });
        }
        [HttpGet("me")]
[Authorize]
public async Task<IActionResult> ObtenerDatosPropios()
{
    var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
    var usuario = await _userService.ObtenerPorIdConUnidad(Guid.Parse(userId));

    if (usuario == null)
        return NotFound();

    return Ok(new
    {
        usuario.Id,
        usuario.NombreCompleto,
        usuario.Tipo,
        usuario.Rama,
        unidad = usuario.Unidad != null ? new
        {
            usuario.Unidad.Id,
            usuario.Unidad.CodigoUnidad
        } : null
    });
}

        [HttpGet("todos")]
        public async Task<ActionResult<List<User>>> ObtenerTodos()
        {
            var usuarios = await _userService.ObtenerUsuarios();
            return Ok(usuarios);
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
