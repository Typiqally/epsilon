using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions.Export;
using Epsilon.Abstractions.Model;
using Microsoft.Extensions.Options;

namespace Epsilon.Export.Exporters;

public class WordModuleExporter : ICanvasModuleExporter
{
    private readonly ExportOptions _options;

    public WordModuleExporter(IOptions<ExportOptions> options)
    {
        _options = options.Value;
    }

    public IEnumerable<string> Formats { get; } = new[] { "word", "docx" };

    public async Task Export(ExportData data, string format)
    {
        using var document = WordprocessingDocument.Create($"{_options.FormattedOutputName}.docx",
            WordprocessingDocumentType.Document);
        document.AddMainDocumentPart();
        document.MainDocumentPart!.Document = new Document(new Body());
        var body = document.MainDocumentPart.Document.Body;

        foreach (var module in data.CourseModules)
        {
            Table table = new Table();

            table.AppendChild<TableProperties>(GetTableProperties());
            table.Append(AddHeader());

            foreach (var kpi in module.Kpis)
            {
                TableRow row = new TableRow();
                row.Append(CreateCell($"{kpi.Name} {kpi.Description}"));

                var cellValueBuilder = new StringBuilder();
                var cellValueOutComeResultsBuilder = new StringBuilder();

                foreach (var assignment in kpi.Assignments)
                {
                    cellValueBuilder.AppendLine($"{assignment.Name} {assignment.Url}");
                    cellValueOutComeResultsBuilder.AppendLine(assignment.Score);
                }

                row.Append(CreateCell(cellValueBuilder.ToString()));
                row.Append(CreateCell(cellValueOutComeResultsBuilder.ToString()));

                table.Append(row);
            }

            body?.Append(CreateText(module.Name));
            body?.Append(table);
        }

        document?.Save();
        document?.Close();
    }

    private static TableRow AddHeader()
    {
        var tr = new TableRow();
        tr.Append(CreateCell("KPI's"));
        tr.Append(CreateCell("Assignments"));
        tr.Append(CreateCell("Score"));
        return tr;
    }

    private static Paragraph CreateText(string text)
    {
        return new Paragraph(new Run(new Text(text)));
    }

    private static TableCell CreateCell(string text)
    {
        var tc = new TableCell();
        tc.Append(CreateText(text));
        tc.Append(new TableCellProperties(
            new TableCellWidth { Type = TableWidthUnitValues.Auto }));
        return tc;
    }


    private static TableProperties GetTableProperties()
    {
        var properties = new TableProperties();
        properties.Append(new TableBorders(
            new TopBorder
            {
                Val = new EnumValue<BorderValues>(BorderValues.Single),
                Size = 3
            },
            new BottomBorder
            {
                Val = new EnumValue<BorderValues>(BorderValues.Single),
                Size = 3
            },
            new LeftBorder
            {
                Val = new EnumValue<BorderValues>(BorderValues.Single),
                Size = 3
            },
            new RightBorder
            {
                Val = new EnumValue<BorderValues>(BorderValues.Single),
                Size = 3
            },
            new InsideHorizontalBorder
            {
                Val = new EnumValue<BorderValues>(BorderValues.Single),
                Size = 6
            },
            new InsideVerticalBorder
            {
                Val = new EnumValue<BorderValues>(BorderValues.Single),
                Size = 6
            }));
        return properties;
    }
}