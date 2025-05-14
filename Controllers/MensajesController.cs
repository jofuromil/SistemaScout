using BackendScout.Models;
using BackendScout.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendScout.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MensajesController : ControllerBase
    {
        private readonly MensajeService _mensajeService;

        public MensajesController(MensajeService mensajeService)
        {
            _mensajeService = mensajeService;
        }

        // ✅ Crear mensaje (solo dirigentes)
        [HttpPost("crear")]
[Authorize(Roles = "Dirigente")]
public async Task<IActionResult> Crear([FromBody] CrearMensajeRequest request)
{
    try
    {
        var mensaje = new Mensaje
        {
            Contenido   = request.Contenido,
            UnidadId    = request.UnidadId,
            DirigenteId = request.DirigenteId
        };

        var creado = await _mensajeService.CrearMensaje(mensaje);
        return Ok(creado);
    }
    catch (Exception ex)
    {
        // Devolvemos también la InnerException si existe
        var errorReal = ex.InnerException?.Message ?? ex.Message;
        return BadRequest(new { mensaje = errorReal });
    }
}

        // ✅ Ver mensajes de una unidad
        [HttpGet("por-unidad")]
        [Authorize]
        public async Task<IActionResult> VerPorUnidad([FromQuery] Guid unidadId)
        {
            try
            {
                var mensajes = await _mensajeService.ObtenerMensajesPorUnidad(unidadId);
                return Ok(mensajes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // ✅ Ver mensajes enviados por un dirigente
        [HttpGet("por-dirigente")]
        [Authorize(Roles = "Dirigente")]
        public async Task<IActionResult> VerPorDirigente([FromQuery] Guid dirigenteId)
        {
            try
            {
                var mensajes = await _mensajeService.ObtenerMensajesPorDirigente(dirigenteId);
                return Ok(mensajes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
