using BackendScout.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendScout.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CargaController : ControllerBase
    {
        private readonly CargaObjetivosService _cargaService;

        public CargaController(CargaObjetivosService cargaService)
        {
            _cargaService = cargaService;
        }

        [HttpPost("cargar-excel")]
        public async Task<IActionResult> CargarObjetivos()
        {
            string ruta = Path.Combine(Directory.GetCurrentDirectory(), "ObjetivosRovers.xlsx");

            try
            {
                var cantidad = await _cargaService.CargarDesdeExcel(ruta);
                return Ok(new { mensaje = $"Se cargaron {cantidad} objetivos correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
