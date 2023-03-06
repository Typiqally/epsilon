using Epsilon.Abstractions.Export;
using Epsilon.Abstractions.Model;
using ExcelLibrary.SpreadSheet;
using Microsoft.Extensions.Options;

namespace Epsilon.Export.Exporters;

public class ExcelModuleExporter : ICanvasModuleExporter
{
    private readonly ExportOptions _options;

    public ExcelModuleExporter(IOptions<ExportOptions> options)
    {
        _options = options.Value;
    }

    public IEnumerable<string> Formats { get; } = new[] { "xls", "xlsx", "excel" };

    public async Task Export(ExportData data, string format)
    {
        var workbook = new Workbook();

        foreach (var module in data.CourseModules)
        {
            var worksheet = new Worksheet(module.Name);
            var kpis = module.Kpis;

            worksheet.Cells[0, 0] = new Cell("KPI");
            worksheet.Cells[0, 1] = new Cell("Assignment(s)");
            worksheet.Cells[0, 2] = new Cell("Score");

            for (var i = 0; i < kpis.Count(); i++)
            {
                var kpi = kpis.ElementAt(i);

                worksheet.Cells[i + 1, 0] = new Cell(kpi.Name);
                worksheet.Cells[i + 1, 1] = new Cell(string.Join('\n', kpi.Assignments.Select(a => a.Name)));
                worksheet.Cells[i + 1, 2] = new Cell(string.Join('\n', kpi.Assignments.Select(a => a.Score)));
            }

            worksheet.Cells.ColumnWidth[0, 0] = 500;
            worksheet.Cells.ColumnWidth[0, 1] = 8000;
            worksheet.Cells.ColumnWidth[0, 2] = 8000;

            workbook.Worksheets.Add(worksheet);
        }

        // We're forced to xls because of the older format
        workbook.Save($"{_options.FormattedOutputName}.xls");
    }
}