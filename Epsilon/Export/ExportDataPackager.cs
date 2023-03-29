using System.Diagnostics;
using Epsilon.Abstractions.Export;
using Epsilon.Abstractions.Model;
using Epsilon.Canvas;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;
using Microsoft.Extensions.Options;

namespace Epsilon.Export;

public class ExportDataPackager : IExportDataPackager
{
    private readonly IPageHttpService _pageService;
    private readonly CanvasSettings _canvasSettings;

    public ExportDataPackager(IPageHttpService pageService,
        IOptions<CanvasSettings> canvasSettings)
    {
        _pageService = pageService;
        _canvasSettings = canvasSettings.Value;
    }

    public ExportDataPackager()
    {
        throw new NotImplementedException();
    }

    public async Task<ExportData> GetExportData(IAsyncEnumerable<ModuleOutcomeResultCollection> data)
    {
        var courseId = _canvasSettings.CourseId;
        var personaHtml = await _pageService.GetPageByName(courseId, "front_page");

        var output = new List<CourseModule>();

        await foreach (var item in data.Where(m => m.Collection.OutcomeResults.Any()))
        {
            var module = new CourseModule {Name = item.Module.Name};
            var links = item.Collection.Links;

            Debug.Assert(links != null, nameof(links) + " != null");

            var alignments = links.AlignmentsDictionary;
            var outcomes = links.OutcomesDictionary;

            var moduleOutcomes = new List<CourseOutcome>();

            foreach (var (outcomeId, outcome) in outcomes)
            {
                var assignmentIds = item.Collection.OutcomeResults
                    .Where(o => o.Link.Outcome == outcomeId && o.Grade() != null)
                    .Select(static o => o.Link.Assignment).ToArray();

                if (assignmentIds.Any())
                {
                    var assignments = assignmentIds
                        .Select(assignmentId => new CourseAssignment
                        {
                            Name = alignments[assignmentId!].Name,
                            Url = alignments[assignmentId!].Url.ToString(),
                            Score = item.Collection.OutcomeResults
                                .First(o => o.Link.Outcome == outcomeId && o.Link.Assignment == assignmentId)
                                .Grade() ?? "N/A",
                        })
                        .ToList();

                    moduleOutcomes.Add(new CourseOutcome
                    {
                        Name = outcome.Title,
                        Description = outcome.ShortDescription(),
                        Assignments = assignments!,
                    });
                }
            }

            module.Outcomes = moduleOutcomes;

            output.Add(module);
        }

        return new ExportData {CourseModules = output, PersonaHtml = personaHtml};
    }
}