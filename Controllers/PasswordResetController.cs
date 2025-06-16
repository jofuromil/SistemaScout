using BackendScout.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendScout.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordResetController : ControllerBase
    {
        private readonly PasswordResetService _resetService;
        private readonly UserService _userService;
        private readonly AuthService _authService;

        public PasswordResetController(
            PasswordResetService resetService,
            UserService userService,
            AuthService authService)
        {
            _resetService = resetService;
            _userService = userService;
            _authService = authService;
        }

        public class RestablecerRequest
        {
            public string Codigo { get; set; } = null!;
            public string NuevaPassword { get; set; } = null!;
        }

        [HttpPost("restablecer")]
        public async Task<IActionResult> RestablecerConCodigo([FromBody] RestablecerRequest request)
        {
            var valido = await _resetService.ValidarCodigoAsync(request.Codigo);
            if (!valido)
                return BadRequest(new { mensaje = "El código es inválido o ha expirado." });

            var usuarioId = await _resetService.ObtenerUsuarioPorCodigoAsync(request.Codigo);
            if (usuarioId == null)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            var usuario = await _userService.ObtenerPorId(usuarioId.Value);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            usuario.Password = _authService.HashPassword(request.NuevaPassword);
            await _userService.ActualizarPassword(usuario);
            await _resetService.MarcarComoUsadoAsync(request.Codigo);

            return Ok(new { mensaje = "Contraseña restablecida correctamente." });
        }
    }
}
