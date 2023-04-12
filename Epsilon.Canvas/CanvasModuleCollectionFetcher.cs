using System.Diagnostics;
using Epsilon.Canvas.Abstractions;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;

namespace Epsilon.Canvas;

public class CanvasModuleCollectionFetcher : ICanvasModuleCollectionFetcher
{
    private readonly IModuleHttpService _moduleService;
    private readonly IOutcomeHttpService _outcomeService;

    public CanvasModuleCollectionFetcher(
        IModuleHttpService moduleService,
        IOutcomeHttpService outcomeService
    )
    {
        _moduleService = moduleService;
        _outcomeService = outcomeService;
    }

    public async IAsyncEnumerable<ModuleOutcomeResultCollection> GetAll(int courseId, IEnumerable<string>? allowedModules)
    {
        var response = await _outcomeService.GetResults(courseId, new[] { "outcomes", "alignments" });
        var modules = await _moduleService.GetAll(courseId, new[] { "items" });

        Debug.Assert(response != null, nameof(response) + " != null");
        Debug.Assert(modules != null, nameof(modules) + " != null");

        foreach (var module in modules.ToArray())
        {
            if (allowedModules == null || !allowedModules.Any() || allowedModules.Contains(module.Name))
            {
                Debug.Assert(module.Items != null, "module.Items != null");

                var ids = module.Items.Select(static i => $"assignment_{i.ContentId}");

                Debug.Assert(response.Links?.Alignments != null, "response.Links?.Alignments != null");

                yield return new ModuleOutcomeResultCollection(module, new OutcomeResultCollection(
                    response.OutcomeResults.Where(r => ids.Contains(r.Link.Alignment)),
                    response.Links with { Alignments = response.Links.Alignments.Where(a => ids.Contains(a.Id)) }
                ));
            }
        }
    }
}