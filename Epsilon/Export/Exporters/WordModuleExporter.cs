using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions.Export;
using Epsilon.Canvas.Abstractions.Model;
using Microsoft.Extensions.Options;
using BottomBorder = DocumentFormat.OpenXml.Wordprocessing.BottomBorder;
using InsideHorizontalBorder = DocumentFormat.OpenXml.Wordprocessing.InsideHorizontalBorder;
using InsideVerticalBorder = DocumentFormat.OpenXml.Wordprocessing.InsideVerticalBorder;
using LeftBorder = DocumentFormat.OpenXml.Wordprocessing.LeftBorder;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using RightBorder = DocumentFormat.OpenXml.Wordprocessing.RightBorder;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using TableCell = DocumentFormat.OpenXml.Wordprocessing.TableCell;
using TableCellProperties = DocumentFormat.OpenXml.Wordprocessing.TableCellProperties;
using TableProperties = DocumentFormat.OpenXml.Wordprocessing.TableProperties;
using TableRow = DocumentFormat.OpenXml.Wordprocessing.TableRow;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using TopBorder = DocumentFormat.OpenXml.Wordprocessing.TopBorder;

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


                table.AppendChild<TableProperties>(GetTableProperties());
                table.Append(AddHeader());

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

                document.MainDocumentPart.Document.Body?.Append(new Paragraph(new Run(new Text(item.Module.Name))));
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
        tc.Append(new TableCellProperties(
            new TableCellWidth { Type = TableWidthUnitValues.Auto }));
        return tc;
    }


    public static TableProperties GetTableProperties()
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

        // properties.Append(new TableBackground());
        return properties;
    }
}