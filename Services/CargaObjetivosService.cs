using BackendScout.Data;
using BackendScout.Models;
using ClosedXML.Excel;

namespace BackendScout.Services
{
    public class CargaObjetivosService
    {
        private readonly AppDbContext _context;

        public CargaObjetivosService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CargarDesdeExcel(string rutaArchivo)
        {
            int cantidadAgregados = 0;

            using var workbook = new XLWorkbook(rutaArchivo);
            var hoja = workbook.Worksheet(1); // Primera hoja del archivo

            foreach (var fila in hoja.RowsUsed().Skip(1)) // Saltamos encabezado
            {
                var area = fila.Cell(1).GetString().Trim();
                var descripcion = fila.Cell(2).GetString().Trim();
                var rama = fila.Cell(3).GetString().Trim();
                var nivel = fila.Cell(4).GetString().Trim();

                if (string.IsNullOrWhiteSpace(nivel))
                {
                    nivel = "General";
                }

                var objetivo = new ObjetivoEducativo
                {
                    Area = area,
                    Descripcion = descripcion,
                    Rama = rama,
                    NivelProgresion = nivel,
                    EdadMinima = 18,
                    EdadMaxima = 21
                };

                _context.ObjetivosEducativos.Add(objetivo);
                cantidadAgregados++;
            }

            await _context.SaveChangesAsync();
            return cantidadAgregados;
        }
    }
}

