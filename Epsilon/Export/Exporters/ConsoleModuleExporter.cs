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
            _logger.LogInformation("================ {ModuleName} ================", module.Name);

            foreach (var submission in module.Submissions)
            {
                _logger.LogInformation("Assignment: {Assignment}", submission.Assignment.Name);

                foreach (var rating in submission.RubricAssessment.Ratings)
                {
                    if (rating.Outcome != null)
                    {
                        _logger.LogInformation("- {Outcome}", rating.Outcome?.Title);
                    }
                }
            }
        }
    }
}