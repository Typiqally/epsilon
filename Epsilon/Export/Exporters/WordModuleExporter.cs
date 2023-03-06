using System.Drawing;
using Epsilon.Abstractions.Export;
using Epsilon.Abstractions.Model;
using Microsoft.Extensions.Options;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Epsilon.Export.Exporters;

public class WordModuleExporter : ICanvasModuleExporter
{
    private readonly ExportOptions _options;

    public WordModuleExporter(IOptions<ExportOptions> options)
    {
        _options = options.Value;
    }

    public IEnumerable<string> Formats { get; } = new[] { "word" };

    public async Task Export(ExportData data, string format)
    {
        using var document = DocX.Create($"{_options.FormattedOutputName}.docx");

        document.AddFooters();
        var link = document.AddHyperlink(Constants.ProjectName, Constants.RepositoryUri);

        document.Footers.Odd
            .InsertParagraph("Created with ")
            .AppendHyperlink(link).Color(Color.Blue).UnderlineStyle(UnderlineStyle.singleLine);
        
        foreach (var module in data.CourseModules)
        {
            var table = document.AddTable(1, 3);

            table.Rows[0].Cells[0].Paragraphs[0].Append("KPI");
            table.Rows[0].Cells[1].Paragraphs[0].Append("Assignment(s)");
            table.Rows[0].Cells[2].Paragraphs[0].Append("Score");

            foreach (var kpi in module.Kpis)
            {
                var row = table.InsertRow();
                row.Cells[0].Paragraphs[0].Append(kpi.Name);

                foreach (var assignment in kpi.Assignments)
                {
                    row.Cells[1].Paragraphs[0].Append(assignment.Name);
                    row.Cells[2].Paragraphs[0].Append(assignment.Score);
                } 
            }

            var par = document.InsertParagraph(module.Name);
            par.FontSize(24);
            par.InsertTableAfterSelf(table).InsertPageBreakAfterSelf();
        }

        document.Save();
    }
}