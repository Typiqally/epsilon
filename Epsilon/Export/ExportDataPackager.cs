using System.Diagnostics;
using Epsilon.Abstractions.Export;
using Epsilon.Abstractions.Model;
using Epsilon.Canvas;
using Epsilon.Canvas.Abstractions;
using Epsilon.Canvas.Abstractions.Service;
using Microsoft.Extensions.Options;

namespace Epsilon.Export;

public class ExportDataPackager : IExportDataPackager
{
    private readonly ICanvasModuleCollectionFetcher _moduleCollectionFetcher;
    private readonly IPageHttpService _pageService;
    private readonly ExportOptions _exportOptions;
    private readonly CanvasSettings _canvasSettings;

    public ExportDataPackager(ICanvasModuleCollectionFetcher moduleCollectionFetcher, IPageHttpService pageService,
        IOptions<CanvasSettings> canvasSettings, IOptions<ExportOptions> exportOptions)
    {
        _moduleCollectionFetcher = moduleCollectionFetcher;
        _pageService = pageService;
        _exportOptions = exportOptions.Value;
        _canvasSettings = canvasSettings.Value;
    }

    public async Task<ExportData> GetExportData()
    {
        var modules = _exportOptions.Modules?.Split(",");
        var courseId = _canvasSettings.CourseId;

        var moduleOutcomes = _moduleCollectionFetcher.GetAll(courseId, modules);
        var personaHtml = await _pageService.GetPageByName(courseId, "front_page");

        var output = new List<CourseModule>();

        await foreach (var item in moduleOutcomes.Where(m => m.Collection.OutcomeResults.Any()))
        {
            var module = new CourseModule {Name = item.Module.Name};
            var links = item.Collection.Links;

            Debug.Assert(links != null, nameof(links) + " != null");

            var alignments = links.AlignmentsDictionary;
            var outcomes = links.OutcomesDictionary;

            var moduleKpis = new List<CourseOutcome>();

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

                    moduleKpis.Add(new CourseOutcome
                    {
                        Name = outcome.Title,
                        Description = outcome.ShortDescription(),
                        Assignments = assignments!,
                    });
                }
            }

            module.Kpis = moduleKpis;

            output.Add(module);
        }

        return new ExportData {CourseModules = output, PersonaHtml = personaHtml};
    }
}