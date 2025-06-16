using System.Security.Claims;
using BackendScout.Models;
using BackendScout.Models.Requests;
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
                    Contenido = request.Contenido,
                    UnidadId = request.UnidadId,
                    DirigenteId = request.DirigenteId
                };

                var creado = await _mensajeService.CrearMensaje(mensaje);
                return Ok(creado);
            }
            catch (Exception ex)
            {
                var errorReal = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { mensaje = errorReal });
            }
        }

        // ✅ Enviar mensaje con adjunto
        [HttpPost("con-adjunto")]
        [Authorize(Roles = "Dirigente")]
        public async Task<IActionResult> EnviarMensajeConAdjunto([FromForm] CrearMensajeAdjuntoRequest request)
        {
            try
            {
                var dirigenteId = ObtenerUsuarioIdDesdeToken(User);
                var unidadId = await _mensajeService.ObtenerUnidadIdPorDirigente(dirigenteId);
                if (unidadId == Guid.Empty)
                    return BadRequest(new { mensaje = "No se encontró la unidad del dirigente." });

                string? rutaImagen = null;
                string? rutaArchivo = null;

                // ✅ Guardar imagen
                if (request.Imagen != null && request.Imagen.Length > 0)
                {
                    var nombreImagen = $"{Guid.NewGuid()}_{request.Imagen.FileName}";
                    var rutaLocal = Path.Combine("ArchivosMensajes", "Imagenes");
                    Directory.CreateDirectory(rutaLocal);
                    var path = Path.Combine(rutaLocal, nombreImagen);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await request.Imagen.CopyToAsync(stream);
                    }

                    rutaImagen = $"archivos/Imagenes/{nombreImagen}";
                }

                // ✅ Guardar archivo
                if (request.Archivo != null && request.Archivo.Length > 0)
                {
                    var nombreArchivo = $"{Guid.NewGuid()}_{request.Archivo.FileName}";
                    var rutaLocal = Path.Combine("ArchivosMensajes", "Archivos");
                    Directory.CreateDirectory(rutaLocal);
                    var path = Path.Combine(rutaLocal, nombreArchivo);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await request.Archivo.CopyToAsync(stream);
                    }

                    rutaArchivo = $"archivos/Archivos/{nombreArchivo}";
                }

                var mensaje = new Mensaje
                {
                    Contenido = request.Contenido,
                    UnidadId = unidadId,
                    DirigenteId = dirigenteId,
                    Fecha = DateTime.UtcNow,
                    ExpiraEl = DateTime.UtcNow.AddDays(60),
                    RutaImagen = rutaImagen,
                    RutaArchivo = rutaArchivo
                };

                await _mensajeService.CrearMensaje(mensaje);
                return Ok(new { mensaje = "Mensaje enviado con éxito." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
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

        // ✅ Eliminar mensaje por ID
        [HttpDelete("eliminar/{id}")]
        [Authorize(Roles = "Dirigente")]
        public async Task<IActionResult> Eliminar(Guid id)
        {
            try
            {
                var dirigenteId = ObtenerUsuarioIdDesdeToken(User);
                var eliminado = await _mensajeService.EliminarMensaje(id, dirigenteId);
                if (!eliminado)
                    return NotFound(new { mensaje = "No tienes permiso para eliminar este mensaje o no existe." });

                return Ok(new { mensaje = "Mensaje eliminado correctamente." });
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

        // ✅ Obtener ID del usuario autenticado desde el token
        private Guid ObtenerUsuarioIdDesdeToken(ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
        }
    }
}
