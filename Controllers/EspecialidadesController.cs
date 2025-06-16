
using BackendScout.Models;
using BackendScout.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackendScout.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EspecialidadesController : ControllerBase
    {
        private readonly EspecialidadService _service;
        private readonly EspecialidadImporter _importer;

        public EspecialidadesController(EspecialidadService service, EspecialidadImporter importer)
        {
            _service = service;
            _importer = importer;
        }

        [HttpGet("rama")]
        public async Task<IActionResult> ObtenerPorRama([FromQuery] string rama)
        {
            var lista = await _service.ObtenerPorRamaAsync(rama);
            return Ok(lista);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObtenerPorId(Guid id)
        {
            var esp = await _service.ObtenerPorIdConRequisitosAsync(id);
            if (esp == null) return NotFound();
            return Ok(esp);
        }

        [HttpPost]
        [Authorize(Roles = "Dirigente")]
        public async Task<IActionResult> Crear([FromBody] Especialidad especialidad)
        {
            var nueva = await _service.CrearEspecialidadAsync(especialidad);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = nueva.Id }, nueva);
        }

        [HttpPost("{id:guid}/requisito")]
        [Authorize(Roles = "Dirigente")]
        public async Task<IActionResult> AgregarRequisito(Guid id, [FromBody] CrearRequisitoRequest request)
        {
            var requisito = new Requisito
            {
                Tipo = request.Tipo,
                Texto = request.Texto
            };

            var creado = await _service.AgregarRequisitoAsync(id, requisito);
            return Ok(creado);
        }

        [HttpPost("requisito-cumplido")]
        [Authorize(Roles = "Scout")]
        public async Task<IActionResult> MarcarRequisitoCumplido([FromBody] RequisitoCumplidoRequest request)
        {
            var scoutId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var resultado = await _service.MarcarRequisitoCumplidoAsync(request.RequisitoId, scoutId);
            return Ok(resultado);
        }

        [HttpPut("validar-requisito")]
        [Authorize(Roles = "Dirigente")]
        public async Task<IActionResult> ValidarRequisito([FromBody] ValidarRequisitoRequest request)
        {
            var resultado = await _service.ValidarRequisitoAsync(request.CumplidoId, request.Aprobado);
            return Ok(resultado);
        }

        [HttpGet("mis-avances")]
        [Authorize(Roles = "Scout")]
        public async Task<IActionResult> ObtenerMisAvances()
        {
            var scoutId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var resumen = await _service.ObtenerResumenPorScoutAsync(scoutId);
            return Ok(resumen);
        }

        [HttpGet("avance-especialidad")]
        [Authorize(Roles = "Scout,Dirigente")]
        public async Task<IActionResult> VerAvanceEspecialidad([FromQuery] Guid idEspecialidad, [FromQuery] string? scoutId = null)
        {
            var userId = scoutId ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            var avance = await _service.ObtenerAvanceEspecialidadAsync(idEspecialidad, userId);
            return Ok(avance);
        }

        [HttpGet("scouts-con-avance")]
        [Authorize(Roles = "Dirigente")]
        public async Task<IActionResult> ScoutsConAvance([FromQuery] Guid unidadId)
        {
            var resultado = await _service.ObtenerScoutsConAvancePendienteAsync(unidadId);
            return Ok(resultado);
        }

        [HttpGet("importar")]
        [Authorize(Roles = "Dirigente")]
        public async Task<IActionResult> Importar()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "especialidadesmalla.xlsx");
            await _importer.ImportarDesdeExcel(path);
            return Ok("Importación completada");
        }

        [HttpGet("avance-scout")]
        [Authorize(Roles = "Dirigente")]
        public async Task<IActionResult> ObtenerRequisitosPendientes(string scoutId)
        {
            var pendientes = await _service.ObtenerRequisitosPendientesPorScout(scoutId);
            return Ok(pendientes);
        }

        [HttpGet("resumen-por-scout")]
        [Authorize(Roles = "Dirigente")]
        public async Task<IActionResult> ObtenerResumenPorUnidad([FromQuery] Guid unidadId)
        {
            try
            {
                var resumen = await _service.ObtenerResumenDeEspecialidadesPorScoutAsync(unidadId);
                return Ok(resumen);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("resumen-scout/{scoutId}")]
        [Authorize(Roles = "Dirigente")]
        public async Task<IActionResult> ObtenerResumenEspecialidadesScout(Guid scoutId)
        {
            var resumen = await _service.ObtenerResumenScout(scoutId);
            return Ok(resumen);
        }
    }
}
