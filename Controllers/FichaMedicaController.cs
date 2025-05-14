using BackendScout.Models;
using BackendScout.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendScout.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FichaMedicaController : ControllerBase
    {
        private readonly FichaMedicaService _service;

        public FichaMedicaController(FichaMedicaService service)
        {
            _service = service;
        }

        [HttpPost("guardar")]
public async Task<IActionResult> GuardarFicha([FromBody] FichaMedica ficha)
{
    try
    {
        // Validar que el usuario solo edite su propia ficha
        var usuarioIdDesdeCliente = ficha.UsuarioId.ToString();
        var usuarioIdDesdeToken = ficha.UsuarioId.ToString(); // Aquí luego se puede obtener del login real

        if (usuarioIdDesdeCliente != usuarioIdDesdeToken)
            return Unauthorized(new { mensaje = "Solo puedes modificar tu propia ficha médica." });

        var resultado = await _service.GuardarFicha(ficha);
        return Ok(resultado);
    }
    catch (Exception ex)
    {
        return BadRequest(new { mensaje = ex.Message });
    }
}


        [HttpGet("ver/{usuarioId}")]
        public async Task<IActionResult> VerFicha(Guid usuarioId)
        {
            var ficha = await _service.ObtenerFicha(usuarioId);
            if (ficha == null)
                return NotFound(new { mensaje = "Ficha no encontrada." });

            return Ok(ficha);
        }

         [HttpGet("ver-como-dirigente")]
public async Task<IActionResult> VerFichaDirigente([FromQuery] Guid dirigenteId, [FromQuery] Guid usuarioId)
{
    try
    {
        var ficha = await _service.VerFichaComoDirigente(dirigenteId, usuarioId);
        if (ficha == null)
            return NotFound(new { mensaje = "Ficha no encontrada." });

        return Ok(ficha);
    }
    catch (Exception ex)
    {
        return BadRequest(new { mensaje = ex.Message });
    }
}
    }
   

}
