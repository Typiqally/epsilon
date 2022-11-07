using Epsilon.Canvas.Abstractions;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;
using Microsoft.Extensions.Logging;

namespace Epsilon.Canvas;

public class CanvasModuleCollectionFetcher : ICanvasModuleCollectionFetcher
{
    private readonly ILogger<CanvasModuleCollectionFetcher> _logger;
    private readonly IModuleHttpService _moduleService;
    private readonly IOutcomeHttpService _outcomeService;

    public CanvasModuleCollectionFetcher(
        ILogger<CanvasModuleCollectionFetcher> logger,
        IModuleHttpService moduleService,
        IOutcomeHttpService outcomeService
    )
    {
        _logger = logger;
        _moduleService = moduleService;
        _outcomeService = outcomeService;
    }

    public async Task<IEnumerable<Module>> GetAll(int courseId)
    {
        _logger.LogInformation("Downloading results...");

        var response = await _outcomeService.GetResults(courseId, new[] { "outcomes", "alignments" });

        var alignments = response.Links.Alignments
            .DistinctBy(static a => a.Id)
            .ToDictionary(static a => a.Id, static a => a);

        var outcomes = response.Links.Outcomes
            .DistinctBy(static o => o.Id)
            .ToDictionary(static o => o.Id.ToString(), static o => o);

        var modules = await _moduleService.GetAll(courseId, new[] { "items" });
        foreach (var module in modules)
        {
            var ids = module.Items.Select(static i => $"assignment_{i.ContentId}");

            module.Collection = new OutcomeResultCollection(
                response.OutcomeResults.Where(r => ids.Contains(r.Link.Alignment)),
                response.Links with { Alignments = response.Links.Alignments.Where(a => ids.Contains(a.Id)) }
            );
        }

        return modules;
    }
}