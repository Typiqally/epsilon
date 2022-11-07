using System.Diagnostics;
using Epsilon.Abstractions.Export;
using Epsilon.Canvas.Abstractions.Model;
using Microsoft.Extensions.Logging;

namespace Epsilon.Export.Exporters;

public class ConsoleModuleExporter : ICanvasModuleExporter
{
    private readonly ILogger<ConsoleModuleExporter> _logger;

    public ConsoleModuleExporter(ILogger<ConsoleModuleExporter> logger)
    {
        _logger = logger;
    }

    public IEnumerable<string> Formats { get; } = new[] { "console", "logs" };

    public async Task Export(IAsyncEnumerable<ModuleOutcomeResultCollection> data, string format)
    {
        await foreach (var item in data)
        {
            _logger.LogInformation("Module: {Name}", item.Module.Name);

            var links = item.Collection.Links;
            var alignments = links?.AlignmentsDictionary;
            var outcomes = links?.OutcomesDictionary;

            Debug.Assert(alignments != null, nameof(alignments) + " != null");
            Debug.Assert(outcomes != null, nameof(outcomes) + " != null");
            
            foreach (var alignment in alignments.Values)
            {
                _logger.LogInformation("Alignment: {Alignment}", alignment.Name);

                foreach (var result in item.Collection.OutcomeResults.Where(o => o.Link.Alignment == alignment.Id && o.Link.Outcome != null))
                {
                    _logger.LogInformation("- {OutcomeName} {Score}", outcomes[result.Link.Outcome!].Title, result.Grade());
                }
            }
        }
    }
}