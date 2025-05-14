using BackendScout.Data;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BackendScout.Services
{
    public class PdfService
    {
        private readonly AppDbContext _context;

        public PdfService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<byte[]> GenerarPdfPorScout(Guid usuarioId)
        {
            var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Id == usuarioId);
            if (usuario == null)
                throw new Exception("Scout no encontrado.");

            var objetivos = await _context.ObjetivosSeleccionados
                .Where(s => s.UsuarioId == usuarioId && s.Validado)
                .Include(s => s.ObjetivoEducativo)
                .ToListAsync();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header()
                        .Text($"Historial de Objetivos Validados\n{usuario.NombreCompleto}")
                        .FontSize(18).Bold();

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Área").Bold();
                            header.Cell().Text("Descripción").Bold();
                        });

                        foreach (var objetivo in objetivos)
                        {
                            table.Cell().Text(objetivo.ObjetivoEducativo.Area);
                            table.Cell().Text(objetivo.ObjetivoEducativo.Descripcion);
                        }
                    });

                    page.Footer().AlignCenter().Text(txt =>
                    {
                        txt.Span("Generado el ").SemiBold();
                        txt.Span(DateTime.Now.ToString("dd/MM/yyyy"));
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
