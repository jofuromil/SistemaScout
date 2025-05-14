using BackendScout.Data;
using BackendScout.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BackendScout.Services
{
    public class PdfObjetivosService
    {
        private readonly AppDbContext _context;

        public PdfObjetivosService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<byte[]> GenerarPdfPorScout(Guid usuarioId)
        {
            var user = await _context.Users.FindAsync(usuarioId);
            if (user == null) throw new Exception("Usuario no encontrado.");

            var objetivos = await _context.ObjetivosSeleccionados
                .Include(s => s.ObjetivoEducativo)
                .Where(s => s.UsuarioId == usuarioId)
                .ToListAsync();

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text($"Objetivos educativos - {user.NombreCompleto}").FontSize(20).Bold();
                    page.Content().Table(tabla =>
                    {
                        tabla.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(5);
                            columns.ConstantColumn(100);
                        });

                        tabla.Header(header =>
                        {
                            header.Cell().Text("Área").Bold();
                            header.Cell().Text("Objetivo").Bold();
                            header.Cell().Text("Validado").Bold();
                        });

                        foreach (var obj in objetivos)
                        {
                            tabla.Cell().Text(obj.ObjetivoEducativo.Area);
                            tabla.Cell().Text(obj.ObjetivoEducativo.Descripcion);
                            tabla.Cell().Text(obj.Validado ? "Sí" : "No");
                        }
                    });
                    page.Footer().AlignCenter().Text($"Generado: {DateTime.Now:dd/MM/yyyy}");
                });
            });

            return pdf.GeneratePdf();
        }
    }
}
