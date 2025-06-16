using BackendScout.Data;
using BackendScout.Models;
using ClosedXML.Excel;

namespace BackendScout.Services
{
    public class EspecialidadImporter
    {
        private readonly AppDbContext _context;

        public EspecialidadImporter(AppDbContext context)
        {
            _context = context;
        }

        public async Task ImportarDesdeExcel(string filePath)
        {
            using var workbook = new XLWorkbook(filePath);
            var worksheet = workbook.Worksheets.First();
            var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // omitir encabezado

            var agrupadas = rows
                .GroupBy(r => new
                {
                    Nombre = r.Cell(1).GetString().Trim(),
                    Rama = r.Cell(2).GetString().Trim(),
                    Descripcion = r.Cell(3).GetString().Trim()
                });

            foreach (var grupo in agrupadas)
            {
                var yaExiste = _context.Especialidades.Any(e =>
                    e.Nombre == grupo.Key.Nombre && e.Rama == grupo.Key.Rama);

                if (yaExiste) continue;

                var especialidad = new Especialidad
                {
                    Nombre = grupo.Key.Nombre,
                    Rama = grupo.Key.Rama,
                    Descripcion = grupo.Key.Descripcion
                };

                foreach (var fila in grupo)
                {
                    var texto = fila.Cell(4).GetString().Trim();
                    if (!string.IsNullOrWhiteSpace(texto))
                    {
                        especialidad.Requisitos.Add(new Requisito
                        {
                            Texto = texto,
                            Tipo = TipoRequisito.Basico
                        });
                    }
                }

                _context.Especialidades.Add(especialidad);
            }

            await _context.SaveChangesAsync();
        }
    }
}
