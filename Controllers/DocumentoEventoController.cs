using BackendScout.DTOs;
using BackendScout.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendScout.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentoEventoController : ControllerBase
    {
        private readonly DocumentoEventoService _documentoService;

        public DocumentoEventoController(DocumentoEventoService documentoService)
        {
            _documentoService = documentoService;
        }

        [HttpPost("subir")]
        public async Task<IActionResult> SubirArchivo([FromForm] SubidaArchivoEventoRequest request)
        {
            try
            {
                var mensaje = await _documentoService.SubirArchivoAsync(
                    request.EventoId,
                    request.DirigenteId,
                    request.Archivo
                );
                return Ok(new { mensaje });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("listar/{eventoId}")]
        public async Task<IActionResult> ListarArchivosEvento(int eventoId)
        {
            var archivos = await _documentoService.ListarArchivosDeEventoAsync(eventoId);

            var resultado = archivos.Select(a => new
            {
                a.Id,
                a.NombreArchivo,
                a.TipoMime,
                a.FechaSubida,
                url = a.RutaArchivo
            });

            return Ok(resultado);
        }
        [HttpPost("agregar-enlace")]
public async Task<IActionResult> AgregarEnlace([FromBody] AgregarEnlaceDocumentoRequest request)
{
    try
    {
        var mensaje = await _documentoService.AgregarEnlaceExternoAsync(
            request.EventoId,
            request.DirigenteId,
            request.Nombre,
            request.Url,
            request.TipoMime
        );

        return Ok(new { mensaje });
    }
    catch (UnauthorizedAccessException ex)
    {
        return StatusCode(403, new { mensaje = ex.Message });
    }
    catch (Exception ex)
    {
        return BadRequest(new { mensaje = ex.Message });
    }
}

    }
}
