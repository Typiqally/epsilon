using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions.Export;
using Epsilon.Canvas.Abstractions.Model;
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

    public async Task Export(IAsyncEnumerable<ModuleOutcomeResultCollection> data, string format)
    {
        using var document = WordprocessingDocument.Create($"{_options.FormattedOutputName}.docx",
            WordprocessingDocumentType.Document);
        document.AddMainDocumentPart();
        document.MainDocumentPart!.Document = new Document(new Body());
        var body = document.MainDocumentPart.Document.Body;

        await foreach (var item in data.Where(static m => m.Collection.OutcomeResults.Any()))
        {
            var links = item.Collection.Links;

            var alignments = links?.AlignmentsDictionary;
            var outcomes = links?.OutcomesDictionary;

            Table table = new Table();

            table.AppendChild<TableProperties>(GetTableProperties());
            table.Append(AddHeader());

            foreach (var (outcomeId, outcome) in outcomes!)
            {
                var assignmentIds = item.Collection.OutcomeResults
                    .Where(o => o.Link.Outcome == outcomeId && o.Grade() != null)
                    .Select(static o => o.Link.Assignment)
                    .ToArray();

                if (assignmentIds.Any())
                {
                    TableRow row = new TableRow();
                    row.Append(CreateCell($"{outcome.Title} {outcome.ShortDescription()}"));

                    var cellValueBuilder = new StringBuilder();

                    foreach (var (_, alignment) in alignments?.Where(a => assignmentIds.Contains(a.Key))!)
                    {
                        cellValueBuilder.AppendLine($"{alignment.Name} {alignment.Url}");
                    }

                    row.Append(CreateCell(cellValueBuilder.ToString()));

                    var cellValueOutComeResultsBuilder = new StringBuilder();
                    foreach (var outcomeResult in item.Collection.OutcomeResults.Where(result =>
                                 result.Link.Outcome == outcomeId))
                    {
                        cellValueOutComeResultsBuilder.AppendLine(outcomeResult.Grade());
                    }

                    row.Append(CreateCell(cellValueOutComeResultsBuilder.ToString()));
                    table.Append(row);
                }
            }

            body?.Append(CreateText(item.Module.Name));
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