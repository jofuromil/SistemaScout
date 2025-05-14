using Microsoft.AspNetCore.Http;

namespace BackendScout.DTOs
{
    public class SubidaArchivoEventoRequest
    {
        public int EventoId { get; set; }
        public Guid DirigenteId { get; set; }
        public IFormFile Archivo { get; set; }
    }
}
