using System.Drawing;
using System.Text;
using Epsilon.Abstractions.Export;
using Epsilon.Canvas.Abstractions.Model;
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

    public void Export(IEnumerable<Module> modules, string format)
    {
        using (DocX document = DocX.Create(_options.FormattedOutputName + ".docx"))
        {
            document.AddFooters();
            var link = document.AddHyperlink("Epsilon", new Uri("https://github.com/Typiqally/epsilon"));

            document.Footers.Odd
                .InsertParagraph("Created with ")
                .AppendHyperlink(link).Color(Color.Blue).UnderlineStyle(UnderlineStyle.singleLine);

            foreach (var module in modules.Where(static m => m.Collection.OutcomeResults.Any()))
            {
                var links = module.Collection.Links;
                var alignments = links.AlignmentsDictionary;
                var outcomes = links.OutcomesDictionary;

                var table = document.AddTable(1, 3);

                table.Rows[0].Cells[0].Paragraphs[0].Append("KPI");
                table.Rows[0].Cells[1].Paragraphs[0].Append("Assignment(s)");
                table.Rows[0].Cells[2].Paragraphs[0].Append("Score");

                foreach (var (outcomeId, outcome) in outcomes)
                {
                    var assignmentIds = module.Collection.OutcomeResults
                        .Where(o => o.Link.Outcome == outcomeId)
                        .Select(static o => o.Link.Assignment)
                        .ToArray();

                    if (assignmentIds.Any())
                    {
                        var row = table.InsertRow();
                        row.Cells[0].Paragraphs[0].Append(outcome.Title + " " + outcome.ShortDescription());

                        var cellValueBuilder = new StringBuilder();

                        foreach (var (alignmentId, alignment) in alignments.Where(a => assignmentIds.Contains(a.Key)))
                        {
                            cellValueBuilder.AppendLine($"{alignment.Name} {alignment.Url}");
                        }

                        row.Cells[1].Paragraphs[0].Append(cellValueBuilder.ToString());

                        var cellValueOutComeResultsBuilder = new StringBuilder();
                        foreach (var outcomeResult in module.Collection.OutcomeResults.Where(result =>
                                     result.Link.Outcome == outcomeId))
                        {
                            cellValueOutComeResultsBuilder.AppendLine(outcomeResult.Grade());
                        }

                        row.Cells[2].Paragraphs[0].Append(cellValueOutComeResultsBuilder.ToString());
                    }
                }

                var par = document.InsertParagraph(module.Name);
                par.FontSize(24);
                par.InsertTableAfterSelf(table).InsertPageBreakAfterSelf();
            }

            document.Save();
        }
    }
}