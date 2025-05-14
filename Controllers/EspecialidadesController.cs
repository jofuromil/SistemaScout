using BackendScout.Models;
using BackendScout.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BackendScout.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EspecialidadesController : ControllerBase
    {
        private readonly EspecialidadService _service;

        public EspecialidadesController(EspecialidadService service)
        {
            _service = service;
        }

        // GET api/Especialidades/rama?rama=Lobatos
        [HttpGet("rama")]
        public async Task<IActionResult> ObtenerPorRama([FromQuery] string rama)
        {
            var lista = await _service.ObtenerPorRamaAsync(rama);
            return Ok(lista);
        }

        // GET api/Especialidades/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObtenerPorId(Guid id)
        {
            var esp = await _service.ObtenerPorIdAsync(id);
            if (esp == null) return NotFound();
            return Ok(esp);
        }

        // POST api/Especialidades
        // (Opcionalmente restringir creando sólo dirigentes)
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Especialidad especialidad)
        {
            var nueva = await _service.CrearEspecialidadAsync(especialidad);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = nueva.Id }, nueva);
        }

        // POST api/Especialidades/{id}/requisito
        [HttpPost("{id:guid}/requisito")]
    public async Task<IActionResult> AgregarRequisito(
    Guid id,
    [FromBody] CrearRequisitoRequest request)
    {
    // Construimos la entidad a partir del DTO
    var requisito = new Requisito
    {
        Tipo            = request.Tipo,
        Texto           = request.Texto,
        // EspecialidadId lo asigna el servicio
    };

    var creado = await _service.AgregarRequisitoAsync(id, requisito);
    return Ok(creado);
    }
    }
}
