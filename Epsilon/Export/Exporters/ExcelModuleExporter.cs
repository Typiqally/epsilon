using System.Text;
using System.Text.RegularExpressions;
using Epsilon.Abstractions.Export;
using Epsilon.Canvas.Abstractions.Model;
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

    public void Export(IEnumerable<Module> modules, string format)
    {
        var workbook = new Workbook();

        foreach (var module in modules.Where(static m => m.Collection.OutcomeResults.Any()))
        {
            var worksheet = new Worksheet(module.Name);

            //Because reasons @source https://stackoverflow.com/a/8127642 
            for (var i = 0; i < 100; i++)
            {
                worksheet.Cells[i, 0] = new Cell("");
            }

            var links = module.Collection.Links;
            var alignments = links.AlignmentsDictionary;
            var outcomes = links.OutcomesDictionary;

            //Add headers
            worksheet.Cells[0, 0] = new Cell("KPI");
            worksheet.Cells[0, 1] = new Cell("Assignment(s)");
            worksheet.Cells[0, 2] = new Cell("Score");

            //Adding all the outcomes. 

            var index = 1;
            foreach (var (outcomeId, outcome) in outcomes)
            {
                var assignmentIds = module.Collection.OutcomeResults
                    .Where(o => o.Link.Outcome == outcomeId)
                    .Select(static o => o.Link.Assignment)
                    .ToArray();

                if (assignmentIds.Any())
                {
                    worksheet.Cells[index, 0] = new Cell(outcome.Title + " " +  ShortDescription(ConvertHtmlToRaw(outcome.Description)));

                    var cellValueBuilder = new StringBuilder();

                    foreach (var (alignmentId, alignment) in alignments.Where(a => assignmentIds.Contains(a.Key)))
                    {
                        cellValueBuilder.AppendLine($"{alignment.Name} {alignment.Url}");
                    }
                    worksheet.Cells[index, 1] = new Cell(cellValueBuilder.ToString());
                    
                    var cellValueOutComeResultsBuilder = new StringBuilder();
                    foreach (var outcomeResult in module.Collection.OutcomeResults.Where(result =>  result.Link.Outcome == outcomeId))
                    {
                        cellValueOutComeResultsBuilder.AppendLine(OutcomeToText(outcomeResult.Score));
                    }

                    worksheet.Cells[index, 2] = new Cell(cellValueOutComeResultsBuilder.ToString());
                    index++;
                }
            }

            worksheet.Cells.ColumnWidth[0, 0] = 500;
            worksheet.Cells.ColumnWidth[0, 1] = 8000;
            worksheet.Cells.ColumnWidth[0, 2] = 8000;

            workbook.Worksheets.Add(worksheet);
        }

        // We're forced to xls because of the older format
        workbook.Save($"{_options.FormattedOutputName}.xls");
    }

    private static string ShortDescription(string description)
    {
        //Function gives only the short English description back of the outcome. 
        var startPos = description.IndexOf(" EN ", StringComparison.Ordinal) + " EN ".Length;
        var endPos = description.IndexOf(" NL ", StringComparison.Ordinal);

        return description.Substring(startPos, endPos - startPos);
    }

    private static string ConvertHtmlToRaw(string html)
    {
        var raw = Regex.Replace(html, "<.*?>", " ");
        var trimmed = Regex.Replace(raw, @"\s\s+", " ");

        return trimmed;
    }

    private string OutcomeToText(double? result)
    {
        switch (result)
        {
            default:
            case 0:
                return "Unsatisfactory";
                break;
            case 3:
                return "Satisfactory";
                break;
            case 4:
                return "Good";
                break;
            case 5:
                return "Outstanding";
                break;
        }
    }
}