using System.Diagnostics;
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

    public async Task Export(IAsyncEnumerable<ModuleOutcomeResultCollection> data, string format)
    {
        using var document = DocX.Create($"{_options.FormattedOutputName}.docx");
        
        document.AddFooters();
        var link = document.AddHyperlink(Constants.ProjectName, Constants.RepositoryUri);

        document.Footers.Odd
            .InsertParagraph("Created with ")
            .AppendHyperlink(link).Color(Color.Blue).UnderlineStyle(UnderlineStyle.singleLine);

        await foreach (var item in data.Where(static m => m.Collection.OutcomeResults.Any()))
        {
            var links = item.Collection.Links;
            
            Debug.Assert(links != null, nameof(links) + " != null");
            
            var alignments = links.AlignmentsDictionary;
            var outcomes = links.OutcomesDictionary;

            var table = document.AddTable(1, 3);

            table.Rows[0].Cells[0].Paragraphs[0].Append("KPI");
            table.Rows[0].Cells[1].Paragraphs[0].Append("Assignment(s)");
            table.Rows[0].Cells[2].Paragraphs[0].Append("Score");

            foreach (var (outcomeId, outcome) in outcomes)
            {
                var assignmentIds = item.Collection.OutcomeResults
                    .Where(o => o.Link.Outcome == outcomeId && o.Grade() != null)
                    .Select(static o => o.Link.Assignment)
                    .ToArray();

                if (assignmentIds.Any())
                {
                    var row = table.InsertRow();
                    row.Cells[0].Paragraphs[0].Append(outcome.Title + " " + outcome.ShortDescription());

                    var cellValueBuilder = new StringBuilder();

                    foreach (var (_, alignment) in alignments.Where(a => assignmentIds.Contains(a.Key)))
                    {
                        cellValueBuilder.AppendLine($"{alignment.Name} {alignment.Url}");
                    }

                    row.Cells[1].Paragraphs[0].Append(cellValueBuilder.ToString());

                    var cellValueOutComeResultsBuilder = new StringBuilder();
                    foreach (var outcomeResult in item.Collection.OutcomeResults.Where(result =>
                                 result.Link.Outcome == outcomeId))
                    {
                        cellValueOutComeResultsBuilder.AppendLine(outcomeResult.Grade());
                    }

                    row.Cells[2].Paragraphs[0].Append(cellValueOutComeResultsBuilder.ToString());
                }
            }

            var par = document.InsertParagraph(item.Module.Name);
            par.FontSize(24);
            par.InsertTableAfterSelf(table).InsertPageBreakAfterSelf();
        }

        document.Save();
    }
}