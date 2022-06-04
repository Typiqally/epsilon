using Epsilon.Abstractions.Export;
using Epsilon.Canvas.Abstractions.Data;
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

            LogAssignments(module.Assignments);
        }
    }

    private void LogAssignments(IEnumerable<Assignment> assignments)
    {
        foreach (var assignment in assignments)
        {
            _logger.LogInformation("{AssignmentName}", assignment.Name);

            LogOutcomeResults(assignment.OutcomeResults);
        }
    }

    private void LogOutcomeResults(IEnumerable<OutcomeResult> results)
    {
        foreach (var outcomeResult in results)
        {
            _logger.LogInformation("\t- {OutcomeTitle}", outcomeResult.Outcome?.Title);
        }
    }
}