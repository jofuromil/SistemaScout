using Microsoft.AspNetCore.Http;

namespace BackendScout.Models.Requests
{
    public class CrearMensajeAdjuntoRequest
    {
        public string Contenido { get; set; } = string.Empty;

        public IFormFile? Imagen { get; set; }

        public IFormFile? Archivo { get; set; }
    }
}
