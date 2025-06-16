using BackendScout.Models;
using BackendScout.Services;
using BackendScout.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BackendScout.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnidadesController : ControllerBase
    {
        private readonly UnidadService _unidadService;

        public UnidadesController(UnidadService unidadService)
        {
            _unidadService = unidadService;
        }

        [HttpPost("crear")]
        [Authorize(Roles = "Dirigente")]
        public async Task<IActionResult> CrearUnidad([FromBody] CrearUnidadRequest request)
        {
            try
            {
                var nuevaUnidad = await _unidadService.CrearUnidad(request);

                return Ok(new
                {
                    id = nuevaUnidad.Id,
                    codigo = nuevaUnidad.CodigoUnidad,  // ✅ Código corto para mostrar
                    nombre = nuevaUnidad.Nombre,
                    rama = nuevaUnidad.Rama
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("todas")]
        public async Task<ActionResult<List<Unidad>>> ObtenerUnidades()
        {
            var unidades = await _unidadService.ObtenerUnidades();
            return Ok(unidades);
        }

        [HttpGet("grupos-por-distrito")]
        public IActionResult ObtenerGruposPorDistrito()
        {
            return Ok(GruposPorDistrito.Grupos);
        }
    }
}
