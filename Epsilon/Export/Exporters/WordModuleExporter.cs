using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation;
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

    public IEnumerable<string> Formats { get; } = new[] { "word" };

    public async Task Export(IAsyncEnumerable<ModuleOutcomeResultCollection> data, string format)
    {
        string fileName = $"{_options.FormattedOutputName}.docx";
        using (var document = WordprocessingDocument.Create(fileName,
                   WordprocessingDocumentType.Document))
        {
            document.AddMainDocumentPart();
            document.MainDocumentPart!.Document = new Document(new Body());


            await foreach (var item in data.Where(static m => m.Collection.OutcomeResults.Any()))
            {
                var links = item.Collection.Links;

                var alignments = links.AlignmentsDictionary;
                var outcomes = links.OutcomesDictionary;
                
                Table table = new Table();

                table.Append(AddHeader());

                //Adding all the outcomes. 
                foreach (var (outcomeId, outcome) in outcomes)
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

                        foreach (var (_, alignment) in alignments.Where(a => assignmentIds.Contains(a.Key)))
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
                document.MainDocumentPart.Document.Body?.Append(table);
            }

            document?.Save();
            document?.Close();
        }
    }

    public static TableRow AddHeader()
    {
        var tr = new TableRow();
        tr.Append(CreateCell("KPI's"));
        tr.Append(CreateCell("Assignments"));
        tr.Append(CreateCell("Score"));
        return tr;
    }

    public static TableCell CreateCell(string text)
    {
        var tc = new TableCell();
        tc.Append(new Paragraph(new Run(new Text(text))));
        return tc;
    }

    public static void AddTable(string fileName, string[,] data)
    {
        using (var document = WordprocessingDocument.Open(fileName, true))
        {
            var doc = document.MainDocumentPart.Document;

            Table table = new Table();

            TableProperties props = new TableProperties(
                new TableBorders(
                    new TopBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 12
                    },
                    new BottomBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 12
                    },
                    new LeftBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 12
                    },
                    new RightBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 12
                    },
                    new InsideHorizontalBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 12
                    },
                    new InsideVerticalBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 12
                    }));

            table.AppendChild<TableProperties>(props);

            for (var i = 0; i <= data.GetUpperBound(0); i++)
            {
                var tr = new TableRow();
                for (var j = 0; j <= data.GetUpperBound(1); j++)
                {
                    var tc = new TableCell();
                    tc.Append(new Paragraph(new Run(new Text(data[i, j]))));

                    // Assume you want columns that are automatically sized.
                    tc.Append(new TableCellProperties(
                        new TableCellWidth { Type = TableWidthUnitValues.Auto }));

                    tr.Append(tc);
                }

                table.Append(tr);
            }

            doc.Body.Append(table);
            doc.Save();
        }
    }
}