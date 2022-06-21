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

    public void Export(IEnumerable<Module> data, string format)
    {
        LogModule(data);
    }

    private void LogModule(IEnumerable<Module> modules)
    {
        foreach (var module in modules)
        {
            _logger.LogInformation("Module: {Name}", module.Name);

            var links = module.Collection.Links;
            var alignments = links.AlignmentsDictionary;
            var outcomes = links.OutcomesDictionary;

            foreach (var alignment in alignments.Values)
            {
                _logger.LogInformation("Alignment: {Alignment}", alignment.Name);

                foreach (var result in module.Collection.OutcomeResults.Where(o => o.Link.Alignment == alignment.Id))
                {
                    _logger.LogInformation("- {OutcomeName} {Score}", outcomes[result.Link.Outcome].Title, result.Score);
                }
            }
        }
    }
}