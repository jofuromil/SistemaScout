using BackendScout.Data;
using BackendScout.Models;
using BackendScout.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BackendScout.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ObjetivoController : ControllerBase
    {
        private readonly ObjetivoService _service;
        private readonly AppDbContext _context;
        private readonly CargaObjetivosService _cargaService;
        private readonly PdfObjetivosService _pdfService; // ✅ NUEVO

        public ObjetivoController(
            ObjetivoService service,
            AppDbContext context,
            CargaObjetivosService cargaService,
            PdfObjetivosService pdfService) // ✅ NUEVO
        {
            _service = service;
            _context = context;
            _cargaService = cargaService;
            _pdfService = pdfService; // ✅ NUEVO
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarPorRamaEdad([FromQuery] string rama, [FromQuery] int edad)
        {
            var objetivos = await _service.ObtenerPorRamaYEdad(rama, edad);
            return Ok(objetivos);
        }

        [HttpPost("seleccionar")]
        public async Task<IActionResult> SeleccionarObjetivo([FromBody] ObjetivoSeleccionado seleccion)
        {
            try
            {
                var resultado = await _service.SeleccionarObjetivo(seleccion.UsuarioId, seleccion.ObjetivoEducativoId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("validar")]
        public async Task<IActionResult> Validar([FromQuery] Guid dirigenteId, [FromQuery] Guid seleccionId)
        {
            try
            {
                var resultado = await _service.ValidarObjetivo(dirigenteId, seleccionId);
                return Ok(new { mensaje = "Objetivo validado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("historial")]
        public async Task<IActionResult> Historial([FromQuery] Guid usuarioId, [FromQuery] bool? soloValidados)
        {
            try
            {
                var historial = await _service.HistorialDeObjetivos(usuarioId, soloValidados);
                return Ok(historial);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("agregar-objetivo")]
        public async Task<IActionResult> AgregarObjetivo([FromBody] ObjetivoEducativo objetivo)
        {
            try
            {
                _context.ObjetivosEducativos.Add(objetivo);
                await _context.SaveChangesAsync();
                return Ok(objetivo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("cargar-excel")]
        public async Task<IActionResult> CargarDesdeExcel([FromQuery] string ruta)
        {
            try
            {
                var cantidad = await _cargaService.CargarDesdeExcel(ruta);
                return Ok(new { mensaje = $"Se cargaron {cantidad} objetivos desde el Excel." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("pendientes/scouts")]
        [Authorize(Roles = "Dirigente")]
        public async Task<IActionResult> ScoutsConPendientes()
        {
            try
            {
                var pendientes = await _service.ObtenerUsuariosConPendientes();
                return Ok(pendientes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("pendientes/por-dirigente")]
        [Authorize(Roles = "Dirigente")]
        public async Task<IActionResult> VerPendientes([FromQuery] Guid dirigenteId)
        {
            try
            {
                var pendientes = await _service.ObtenerPendientesPorDirigente(dirigenteId);
                return Ok(pendientes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("pendientes-scout")]
        [Authorize(Roles = "Dirigente")]
        public async Task<IActionResult> PendientesScout([FromQuery] Guid usuarioId)
        {
            try
            {
                var pendientes = await _service.ObtenerPendientesPorScout(usuarioId);
                return Ok(pendientes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("exportar-pdf")]
        [Authorize(Roles = "Dirigente")]
        public async Task<IActionResult> ExportarPdf([FromQuery] Guid usuarioId)
        {
            try
            {
                var pdfBytes = await _pdfService.GenerarPdfPorScout(usuarioId);
                return File(pdfBytes, "application/pdf", "objetivos_scout.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
